using Appneuron.Services;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Assets.Appneuron.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataModel;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    private string projectId;
    private string customerId;   
    private int timer = 0;


    private IdUnityManager idUnityManager;

    private IKafkaMessageBroker _kafkaMessageBroker;


    private List<string> advTypeList = new List<string> { " ReviewAd",
        "Banner",
        "InterstitialAds",
        "InteractiveAd"
    };

    private void Awake()
    {
        using (var kernel = new StandardKernel())
        {
            kernel.Load(Assembly.GetExecutingAssembly());
            _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
        }
    }
    void Start()
    {
        projectId = ChurnBlockerSingletonConfigService.Instance.GetProjectID();
        customerId = ChurnBlockerSingletonConfigService.Instance.GetCustomerID();
        idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
    }

    async void Update()
    {
        timer += 1;

        if (timer >= 1000)
            Application.Quit();

        var id = idUnityManager.GenerateId();
        var difficultyLevel = UnityEngine.Random.Range(1, 11);
        var playingLevelCount = UnityEngine.Random.Range(3, 251);

        var startedDate = new DateTime(DateTime.Now.Year,
            month: 3, day: UnityEngine.Random.Range(1, 6),
            hour: UnityEngine.Random.Range(1, 23),
            minute: UnityEngine.Random.Range(1, 59),
            0, 0);
        var SessionStart = startedDate;

        for (int i = 0; i < playingLevelCount; i++)
        {

            var isAfterDay = UnityEngine.Random.Range(1, 10) == 5;
            if (isAfterDay)
            {
                startedDate.AddDays(1);
                startedDate.AddHours(UnityEngine.Random.Range(1, 8));    
            }

            startedDate.AddMinutes(1);
            var playingTime = UnityEngine.Random.Range(50, 600);
            await calculateLevelBaseSession(startedDate, startedDate.AddSeconds(playingTime), id, i, difficultyLevel);

            var isdead = UnityEngine.Random.Range(1, 5) == 3;
            if (isdead)
            {
                await isDeadSendData(id, difficultyLevel, i, playingTime);
            }

            await SendData(UnityEngine.Random.Range(50, 5500),
                UnityEngine.Random.Range(50, 5500), 0, false, UnityEngine.Random.Range(500, 5500),
                UnityEngine.Random.Range(500, 5500), playingTime, id, difficultyLevel, i.ToString());

            var isGameSessionFinished = UnityEngine.Random.Range(1, 3) == 2;
            if (isGameSessionFinished)
            {
                await CalculateGameSession(SessionStart: SessionStart,
                    SessionFinish:startedDate, id);
                startedDate.AddHours(UnityEngine.Random.Range(1, 12));
                SessionStart = startedDate;
            }
           


            System.Random r = new System.Random();
            string advType = advTypeList[r.Next(advTypeList.Count)];
            await CalculateAdvEvent(id, i.ToString(), 
                startedDate.AddSeconds(playingTime - UnityEngine.Random.Range(5, 350)), 
                difficultyLevel, 
                advType);
        }


    }

    private async Task isDeadSendData(string id, int difficultyLevel, int i, int playingTime)
    {
        await SendData(UnityEngine.Random.Range(50, 5500),
       UnityEngine.Random.Range(50, 5500), 0, true, UnityEngine.Random.Range(500, 5500),
       UnityEngine.Random.Range(500, 5500), playingTime, id, difficultyLevel, i.ToString());

        var isdeadLocal = UnityEngine.Random.Range(1, 5) == 3;
        if (!isdeadLocal)
            return;
        await isDeadSendData(id, difficultyLevel, i, UnityEngine.Random.Range(50, 600));

    }

    private async Task CalculateGameSession(DateTime SessionStart, DateTime SessionFinish, string id)
    {

        GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel
        {

            ClientId = id,
            ProjectID = projectId,
            CustomerID = customerId,
            SessionStartTime = SessionStart,
            SessionFinishTime = SessionFinish,
            SessionTimeMinute = UnityEngine.Random.Range(50, 600)

        };

        var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
        Debug.Log(result.Success);
    }

    private async Task calculateLevelBaseSession(DateTime SessionStart,
        DateTime SessionFinish,
        string playerId,
        int levelIndex,
        int difficultyLevel)
    {


        LevelBaseSessionDataModel dataModel = new LevelBaseSessionDataModel
        {

            ClientId = playerId,
            ProjectID = projectId,
            CustomerID = customerId,
            levelName = levelIndex.ToString(),
            DifficultyLevel = difficultyLevel,
            SessionStartTime = SessionStart,
            SessionFinishTime = SessionFinish,
            SessionTimeMinute = UnityEngine.Random.Range(50, 600)
        };
        var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
        Debug.Log(result.Success);

    }

   

    private async Task CalculateAdvEvent(string clientId,
        string levelName,
        DateTime trigerdTime,
        int difficultyLevel,
        string advType)
    {

        var clickCount = UnityEngine.Random.Range(1, 5);
        int inTime = 0;

        for (int i = 0; i < clickCount; i++)
        {
            if (i == 0)
            {
                inTime = UnityEngine.Random.Range(5, 15);
            }
            else
            {
                inTime = UnityEngine.Random.Range(inTime + 5, inTime + 15);
            }


            AdvEventDataModel advEventDataModel = new AdvEventDataModel
            {
                ClientId = clientId,
                ProjectID = projectId,
                CustomerID = customerId,
                TrigersInlevelName = levelName,
                AdvType = advType,
                DifficultyLevel = difficultyLevel,
                InMinutes = inTime,
                TrigerdTime = trigerdTime
            };


            var result = await _kafkaMessageBroker.SendMessageAsync(advEventDataModel);
            Debug.Log(result.Success);

        }
    }

    public async Task SendData
          (float TransformX,
          float TransformY,
          float TransformZ,
          bool IsDead,
          int AverageScores,
          int TotalPowerUsage,
          int playingTime,
          string playerId,
          int difficultyLevel,
          string levelName)
    {

        Vector3 transform = new Vector3(TransformX,
         TransformY,
         TransformZ);
        int İsDead = 0;
        if (IsDead)
        {
            İsDead = 1;
            await SendLevelbaseDieDatas
             (levelName,
            playingTime,
             transform, playerId,
             difficultyLevel);
        }

        await SendEveryLoginLevelDatas(levelName,
            playingTime,
            AverageScores,
            İsDead,
            TotalPowerUsage,
            playerId,
            difficultyLevel);

    }

    private async Task SendLevelbaseDieDatas
        (string levelName,
        int minutes,
        Vector3 transform,
        string playerId,
        int difficultyLevel)
    {
        EnemyBaseWithLevelFailDataModel dataModel = new EnemyBaseWithLevelFailDataModel
        {

            ClientId = playerId,
            ProjectID = projectId,
            CustomerID = customerId,
            levelName = levelName,
            DiyingTimeAfterLevelStarting = minutes,
            DiyingDifficultyLevel = difficultyLevel,
            FailLocationX = transform.x,
            FailLocationY = transform.y,
            FailLocationZ = transform.z


        };

        var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
        Debug.Log(result.Success);

    }


    private async Task SendEveryLoginLevelDatas
          (string levelname,
          int minutes,
          int averageScores,
          int isDead,
          int totalPowerUsage,
          string playerId,
          int difficultyLevel)
    {

        EnemyBaseEveryLoginLevelDatasModel dataModel = new EnemyBaseEveryLoginLevelDatasModel
        {

            ClientId = playerId,
            ProjectID = projectId,
            CustomerID = customerId,   
            Levelname = levelname,
            LevelsDifficultylevel = difficultyLevel,
            PlayingTime = minutes,
            AverageScores = averageScores,
            IsDead = isDead,
            TotalPowerUsage = totalPowerUsage

        };

        var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
        Debug.Log(result.Success);

    }



}
