using Assets.Appneuron.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Project.ChurnBlockerModule.ChildModules.DataVisualizationModule.Services.CounterServices;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.Datamodel;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.UnityWorkFlow
{
    public class LevelDatasManager : MonoBehaviour
    {

       private readonly IEveryLoginLevelDal _everyLoginLevelDal;
        private readonly ILevelBaseDieDal _levelBaseDieDal;
        private readonly IRestClientServices _restClientServices;
        private readonly ICryptoServices _cryptoServices;
        public LevelDatasManager(IEveryLoginLevelDal everyLoginLevelDal,
        ILevelBaseDieDal levelBaseDieDal,
        IRestClientServices restClientServices,
        ICryptoServices cryptoServices
)
        {
            _everyLoginLevelDal = everyLoginLevelDal;
            _levelBaseDieDal = levelBaseDieDal;
            _restClientServices = restClientServices;
            _cryptoServices = cryptoServices;
        }



        string playerId;
        string projectId;
        string CustomerId;
        string WebApilink;
        CounterServices counterServices;


        public LevelDatasManager()
        {

        }

        void Start()
        {

            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();

            playerId = IdConfigServices.GetPlayerID();
            projectId = ChurnBlockerConfigServices.GetProjectID();
            CustomerId = ChurnBlockerConfigServices.GetCustomerID();
            WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            StartCoroutine(LateStart(1));
        }

        IEnumerator LateStart(float waitTime)
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
                   (int)(counterServices.levelBaseGameTimer),
                    transform);
                }

                SendEveryLoginLevelDatas(sceneName,
                   (int)(counterServices.levelBaseGameTimer),
                   AverageScores,
                   İsDead,
                   TotalPowerUsage);

        }

        void SendLevelbaseDieDatas
            (string levelName,
            int minutes,
            Vector3 transform)
        {

            string filepath = ComponentsConfigServices.LevelBaseDieDataPath;

            LevelBaseDieDataModel dataModel = new LevelBaseDieDataModel {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                levelName = levelName,
                DiyingTimeAfterLevelStarting = minutes,
                DiyingDifficultyLevel = ComponentsConfigServices.CurrentDifficultyLevel,
                DiyingLocationX = transform.x,
                DiyingLocationY = transform.y,
                DiyingLocationZ = transform.z


            };

            string statuseCode = _restClientServices.Post(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            _levelBaseDieDal.Insert(filepath + fileName, dataModel);
            
        }

        void CheckLevelbaseDieAndSend()
        {
          
            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.LevelBaseDieDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = _levelBaseDieDal.Select(ComponentsConfigServices.LevelBaseDieDataPath + fileName);
                string statuseCode = _restClientServices.Post(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    _levelBaseDieDal.Delete(ComponentsConfigServices.LevelBaseDieDataPath + fileName);
                }
            }
        }

        void SendEveryLoginLevelDatas
            (string levelname,
            int minutes,
            int averageScores,
            int isDead,
            int totalPowerUsage)
        {
            string filepath = ComponentsConfigServices.EveryLoginLevelDatasPath;

            EveryLoginLevelDatasModel dataModel = new EveryLoginLevelDatasModel {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                Levelname = levelname,
                LevelsDifficultylevel = ComponentsConfigServices.CurrentDifficultyLevel,
                PlayingTime = minutes,
                AverageScores = averageScores,
                IsDead = isDead,
                TotalPowerUsage = totalPowerUsage

            };

            string statuseCode =_restClientServices.Post(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            _everyLoginLevelDal.Insert(filepath + fileName, dataModel);
        }


        void CheckEveryLoginLevelDatasAndSend()
        {

            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.EveryLoginLevelDatasModel);
            foreach (var fileName in FolderList)
            {
               var dataModel = _everyLoginLevelDal.Select(ComponentsConfigServices.EveryLoginLevelDatasPath + fileName);
                string statuseCode =_restClientServices.Post(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    _everyLoginLevelDal.Delete(ComponentsConfigServices.EveryLoginLevelDatasPath + fileName);
                }
            }


        }
    }
}