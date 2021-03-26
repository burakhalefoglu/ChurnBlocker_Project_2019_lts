using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.Datamodel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using Ninject;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using Appneuron;
using Appneuron.Services;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.UnityManager
{
    public class LevelDatasManager : MonoBehaviour
    {

        private IEveryLoginLevelDal _everyLoginLevelDal;
        private ILevelBaseDieDal _levelBaseDieDal;
        private IRestClientServices _restClientServices;
        private ICryptoServices _cryptoServices;

        private string playerId;
        private string projectId;
        private string customerId;
        private string webApilink;

        private CounterServices counterServices;
        private DifficultySingletonModel difficultySingletonModel;

        private void Awake()
        {
            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                _everyLoginLevelDal = kernel.Get<IEveryLoginLevelDal>();
                _levelBaseDieDal = kernel.Get<ILevelBaseDieDal>();
                _restClientServices = kernel.Get<IRestClientServices>();
                _cryptoServices = kernel.Get<ICryptoServices>();
            }
        }

        private void Start()
        {
            IdUnityManager idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            difficultySingletonModel = DifficultySingletonModel.Instance;

            playerId = idUnityManager.GetPlayerID();
            projectId = ChurnBlockerConfigService.GetProjectID();
            customerId = ChurnBlockerConfigService.GetCustomerID();
            webApilink = ChurnBlockerConfigService.GetWebApiLink();

            StartCoroutine(LateStart(1));
        }

        private IEnumerator LateStart(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            CheckEveryLoginLevelDatasAndSend();
            CheckLevelbaseDieAndSend();
        }

        public void SendData
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
                SendLevelbaseDieDatas
                (sceneName,
               (int)(counterServices.LevelBaseGameTimer),
                transform);
            }

            SendEveryLoginLevelDatas(sceneName,
               (int)(counterServices.LevelBaseGameTimer),
               AverageScores,
               İsDead,
               TotalPowerUsage);

        }

        private void SendLevelbaseDieDatas
            (string levelName,
            int minutes,
            Vector3 transform)
        {

            string filepath = ComponentsConfigService.LevelBaseDieDataPath;

            LevelBaseDieDataModel dataModel = new LevelBaseDieDataModel
            {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                levelName = levelName,
                DiyingTimeAfterLevelStarting = minutes,
                DiyingDifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel,
                DiyingLocationX = transform.x,
                DiyingLocationY = transform.y,
                DiyingLocationZ = transform.z


            };

            var result = _restClientServices.Post(webApilink, dataModel);

            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            _levelBaseDieDal.Insert(filepath + fileName, dataModel);

        }

        private void CheckLevelbaseDieAndSend()
        {

            List<string> FolderList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService.SaveTypePath.LevelBaseDieDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = _levelBaseDieDal.Select(ComponentsConfigService.LevelBaseDieDataPath + fileName);
                var result = _restClientServices.Post(webApilink, dataModel);
                if (result.Success)
                {
                    _levelBaseDieDal.Delete(ComponentsConfigService.LevelBaseDieDataPath + fileName);
                }
            }
        }

        private void SendEveryLoginLevelDatas
            (string levelname,
            int minutes,
            int averageScores,
            int isDead,
            int totalPowerUsage)
        {

            EveryLoginLevelDatasModel dataModel = new EveryLoginLevelDatasModel
            {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                Levelname = levelname,
                LevelsDifficultylevel = difficultySingletonModel.CurrentDifficultyLevel,
                PlayingTime = minutes,
                AverageScores = averageScores,
                IsDead = isDead,
                TotalPowerUsage = totalPowerUsage

            };

            var result = _restClientServices.Post(webApilink, dataModel);

            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigService.EveryLoginLevelDatasPath + fileName;

            _everyLoginLevelDal.Insert(filepath, dataModel);
        }


        private void CheckEveryLoginLevelDatasAndSend()
        {

            List<string> FolderList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService.SaveTypePath.EveryLoginLevelDatasModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = _everyLoginLevelDal.Select(ComponentsConfigService.EveryLoginLevelDatasPath + fileName);
                var result = _restClientServices.Post(webApilink, dataModel);
                if (result.Success)
                {
                    _everyLoginLevelDal.Delete(ComponentsConfigService.EveryLoginLevelDatasPath + fileName);
                }
            }


        }
    }
}