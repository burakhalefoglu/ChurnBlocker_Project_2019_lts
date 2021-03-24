using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.Project.ChurnBlockerModule.ChildModules.DataVisualizationModule.Services.CounterServices;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.Datamodel;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using Assets.Appneuron.UnityWorkflowBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.UnityWorkFlow
{
    public class LevelDatasManager : MonoBehaviour
    {

        string playerId;
        string projectId;
        string CustomerId;
        string WebApilink;
        CounterServices counterServices;

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

            BaseVisualizationDataManager<LevelBaseDieDataModel> baseDataWorkflow =
               new BaseVisualizationDataManager<LevelBaseDieDataModel>();

            LevelBaseDieDataModel dataModel = new LevelBaseDieDataModel();
            LevelBaseDieDAL dataAccess = new LevelBaseDieDAL();

            string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            baseDataWorkflow.SaveData(filepath, new LevelBaseDieDataModel
            {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                levelName = levelName,
                DiyingTimeAfterLevelStarting = minutes,
                DiyingDifficultyLevel = ComponentsConfigServices.CurrentDifficultyLevel,
                DiyingLocationX = transform.x,
                DiyingLocationY = transform.y,
                DiyingLocationZ = transform.z

            }, dataAccess);
        }

        void CheckLevelbaseDieAndSend()
        {
            BaseVisualizationDataManager<LevelBaseDieDataModel> baseDataWorkflow =
           new BaseVisualizationDataManager<LevelBaseDieDataModel>();

            LevelBaseDieDataModel dataModel = new LevelBaseDieDataModel();
            LevelBaseDieDAL modelDal = new LevelBaseDieDAL();


            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.LevelBaseDieDataModel);
            foreach (var item in FolderList)
            {
                dataModel = modelDal.Select(ComponentsConfigServices.LevelBaseDieDataPath + item, dataModel);
                string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    modelDal.Delete(ComponentsConfigServices.LevelBaseDieDataPath + item);
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




            BaseVisualizationDataManager<EveryLoginLevelDatasModel> baseDataWorkflow =
                new BaseVisualizationDataManager<EveryLoginLevelDatasModel>();

            EveryLoginLevelDatasModel dataModel = new EveryLoginLevelDatasModel();
            EveryLoginLevelDAL dataAccess = new EveryLoginLevelDAL();

            string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            baseDataWorkflow.SaveData(filepath, new EveryLoginLevelDatasModel
            {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                Levelname = levelname,
                LevelsDifficultylevel = ComponentsConfigServices.CurrentDifficultyLevel,
                PlayingTime = minutes,
                AverageScores = averageScores,
                IsDead = isDead,
                TotalPowerUsage = totalPowerUsage

            }, dataAccess);
        }


        void CheckEveryLoginLevelDatasAndSend()
        {
            BaseVisualizationDataManager<EveryLoginLevelDatasModel> baseDataWorkflow =
            new BaseVisualizationDataManager<EveryLoginLevelDatasModel>();

            EveryLoginLevelDatasModel dataModel = new EveryLoginLevelDatasModel();
            EveryLoginLevelDAL modelDal = new EveryLoginLevelDAL();


            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.EveryLoginLevelDatasModel);
            foreach (var item in FolderList)
            {
                dataModel = modelDal.Select(ComponentsConfigServices.EveryLoginLevelDatasPath + item, dataModel);
                string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    modelDal.Delete(ComponentsConfigServices.EveryLoginLevelDatasPath + item);
                }
            }


        }
    }
}