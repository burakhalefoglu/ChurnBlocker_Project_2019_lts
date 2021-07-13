using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using Ninject;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using Appneuron.Models;
using Appneuron.Services;
using System.Threading.Tasks;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager
{
    public class EnemyBaseLevelDatasManager : MonoBehaviour
    {
        private string playerId;
        private string projectId;
        private string customerId;

        private IEnemyBaseEveryLoginLevelDal _everyLoginLevelDal;
        private IEnemyBaseWithLevelDieDal _levelBaseDieDal;
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
                _everyLoginLevelDal = kernel.Get<IEnemyBaseEveryLoginLevelDal>();
                _levelBaseDieDal = kernel.Get<IEnemyBaseWithLevelDieDal>();
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
            await CheckEveryLoginLevelDatasAndSend();
            await CheckLevelbaseDieAndSend();
            localDataService.CheckLocalData += CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData += CheckLevelbaseDieAndSend;
        }

        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData -= CheckLevelbaseDieAndSend;
        }

        public async Task SendData
           (float TransformX,
           float TransformY,
           float TransformZ,
           bool IsDead,
           int AverageScores,
           int TotalPowerUsage)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            Vector3 transform = new Vector3(TransformX,
             TransformY,
             TransformZ);
            int İsDead = 0;
            if (IsDead)
            {
                İsDead = 1;
                await SendLevelbaseDieDatas
                 (sceneName,
                (int)(counterServices.LevelBaseGameTimer),
                 transform);
            }

            await SendEveryLoginLevelDatas(sceneName,
                (int)(counterServices.LevelBaseGameTimer),
                AverageScores,
                İsDead,
                TotalPowerUsage);

        }

        private async Task SendLevelbaseDieDatas
            (string levelName,
            int minutes,
            Vector3 transform)
        {

            string filepath = ComponentsConfigService.LevelBaseDieDataPath;

            EnemyBaseWithLevelFailDataModel dataModel = new EnemyBaseWithLevelFailDataModel
            {

                ClientId = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                levelName = levelName,
                DiyingTimeAfterLevelStarting = minutes,
                DiyingDifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel,
                FailLocationX = transform.x,
                FailLocationY = transform.y,
                FailLocationZ = transform.z


            };

            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            await _levelBaseDieDal.InsertAsync(filepath + fileName, dataModel);

        }

        private async Task CheckLevelbaseDieAndSend()
        {

            List<string> FolderList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService.SaveTypePath.LevelBaseDieDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _levelBaseDieDal.SelectAsync(ComponentsConfigService.LevelBaseDieDataPath + fileName);
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _levelBaseDieDal.DeleteAsync(ComponentsConfigService.LevelBaseDieDataPath + fileName);
                }
            }
        }





        private async Task SendEveryLoginLevelDatas
            (string levelname,
            int minutes,
            int averageScores,
            int isDead,
            int totalPowerUsage)
        {

            EnemyBaseEveryLoginLevelDatasModel dataModel = new EnemyBaseEveryLoginLevelDatasModel
            {

                ClientId = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                Levelname = levelname,
                LevelsDifficultylevel = difficultySingletonModel.CurrentDifficultyLevel,
                PlayingTime = minutes,
                AverageScores = averageScores,
                IsDead = isDead,
                TotalPowerUsage = totalPowerUsage

            };

            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);

            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigService.EveryLoginLevelDatasPath + fileName;

            await _everyLoginLevelDal.InsertAsync(filepath, dataModel);
        }


        private async Task CheckEveryLoginLevelDatasAndSend()
        {

            List<string> FolderList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService.SaveTypePath.EveryLoginLevelDatasModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _everyLoginLevelDal.SelectAsync(ComponentsConfigService.EveryLoginLevelDatasPath + fileName);
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _everyLoginLevelDal.DeleteAsync(ComponentsConfigService.EveryLoginLevelDatasPath + fileName);
                }
            }


        }
    }
}