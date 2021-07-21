using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ninject;
using System.Reflection;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using Appneuron.Models;
using Appneuron.Services;
using System.Threading.Tasks;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.UnityManager
{
    public class SessionDatasManager : MonoBehaviour
    {
        private string playerId;
        private string projectId;
        private string customerId;
        private bool isNewLevel = true;
        private string levelName;


        private IGameSessionEveryLoginDal _gameSessionEveryLoginDal;
        private ILevelBaseSessionDal _levelBaseSessionDal;
        private IKafkaMessageBroker _kafkaMessageBroker;
        private ICryptoServices _cryptoServices;


        private CounterServices counterServices;
        private DifficultySingletonModel difficultySingletonModel;
        private LocalDataService localDataService;



        private void Awake()
        {
            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                _gameSessionEveryLoginDal = kernel.Get<IGameSessionEveryLoginDal>();
                _levelBaseSessionDal = kernel.Get<ILevelBaseSessionDal>();
                _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                _cryptoServices = kernel.Get<ICryptoServices>();
            }
        }

        private async void Start()
        {

            IdUnityManager idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();


            difficultySingletonModel = DifficultySingletonModel.Instance;

            playerId = idUnityManager.GetPlayerID();
            projectId = ChurnBlockerSingletonConfigService.Instance.GetProjectID();
            customerId = ChurnBlockerSingletonConfigService.Instance.GetCustomerID();


            await LateStart(3);
        }
        async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await CheckGameSessionEveryLoginDataAndSend();
            await CheckLevelBaseSessionDataAndSend();
            localDataService.CheckLocalData += CheckGameSessionEveryLoginDataAndSend;
            localDataService.CheckLocalData += CheckLevelBaseSessionDataAndSend;

        }


        private async void OnApplicationQuit()
        {
            DateTime gameSessionEveryLoginFinish = counterServices.GameSessionEveryLoginStart
            .AddSeconds(counterServices.TimerForGeneralSession);
            float minutes = counterServices.TimerForGeneralSession / 60;

            await SendGameSessionEveryLoginData(counterServices.GameSessionEveryLoginStart,
                gameSessionEveryLoginFinish,
                minutes,
                playerId);

            localDataService.CheckLocalData -= CheckGameSessionEveryLoginDataAndSend;
            localDataService.CheckLocalData -= CheckLevelBaseSessionDataAndSend;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (isNewLevel)
            {
                levelName = scene.name;
                isNewLevel = false;
            }
            else
            {
                await SendLevelbaseSessionData(counterServices.LevelBaseGameTimer, levelName, counterServices.LevelBaseGameSessionStart);
                levelName = scene.name;

            }
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }






        public async Task SendLevelbaseSessionData(float sessionSeconds,
            string levelName,
            DateTime levelBaseGameSessionStart)
        {

            string filepath = ComponentsConfigService.LevelBaseSessionDataPath;
            DateTime levelBaseGameSessionFinish = levelBaseGameSessionStart.AddSeconds(sessionSeconds);
            float minutes = sessionSeconds / 60;

            LevelBaseSessionDataModel dataModel = new LevelBaseSessionDataModel
            {

                ClientId = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                levelName = levelName,
                DifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel,
                SessionStartTime = levelBaseGameSessionStart,
                SessionFinishTime = levelBaseGameSessionFinish,
                SessionTimeMinute = minutes
            };
            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            await _levelBaseSessionDal.InsertAsync(filepath + fileName, dataModel);
        }

        private async Task CheckLevelBaseSessionDataAndSend()
        {

            List<string> FolderList = ComponentsConfigService.GetSavedDataFilesNames(ComponentsConfigService.SaveTypePath.LevelBaseSessionDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _levelBaseSessionDal.SelectAsync(ComponentsConfigService.LevelBaseSessionDataPath + fileName);
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _levelBaseSessionDal.DeleteAsync(ComponentsConfigService.LevelBaseSessionDataPath + fileName);
                }
            }

        }











        public async Task SendGameSessionEveryLoginData(DateTime sessionStartTime,
            DateTime sessionFinishTime,
            float minutes,
            string playerId)
        {

            string filepath = ComponentsConfigService.GameSessionEveryLoginDataPath;

            GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel
            {

                ClientId = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                SessionStartTime = sessionStartTime,
                SessionFinishTime = sessionFinishTime,
                SessionTimeMinute = minutes

            };

            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            await _gameSessionEveryLoginDal.InsertAsync(filepath + fileName, dataModel);
        }




        private async Task CheckGameSessionEveryLoginDataAndSend()
        {
            Debug.Log("başarılı bir şekilde gerçekleşti");
            List<string> FolderList = ComponentsConfigService.GetSavedDataFilesNames(ComponentsConfigService.SaveTypePath.GameSessionEveryLoginDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _gameSessionEveryLoginDal.SelectAsync(ComponentsConfigService.GameSessionEveryLoginDataPath + fileName);
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _gameSessionEveryLoginDal.DeleteAsync(ComponentsConfigService.GameSessionEveryLoginDataPath + fileName);
                }
            }


        }




    }
}