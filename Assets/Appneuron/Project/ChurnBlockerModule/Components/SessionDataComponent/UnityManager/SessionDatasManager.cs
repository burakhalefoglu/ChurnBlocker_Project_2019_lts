
using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.Project.ChurnBlockerModule.ChildModules.DataVisualizationModule.Services.CounterServices;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using Assets.Appneuron.UnityWorkflowBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.UnityWorkflow
{
    public class SessionDatasManager : MonoBehaviour
    {
        string playerId;
        string projectId;
        string CustomerId;
        string WebApilink;
        bool IsNewLevel = true;
        string levelName;

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
            CheckGeneralDataAndSend();
            CheckLevelBaseSessionDataAndSend();
        }

        private void OnApplicationQuit()
        {

            SendGeneralSessionData();
            SendDailySessionData();
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (IsNewLevel)
            {
                levelName = scene.name;
                IsNewLevel = false;
            }
            else
            {
                SendLevelbaseSessionData(counterServices.levelBaseGameTimer, levelName, counterServices.levelBaseGameSessionStart);
                levelName = scene.name;

            }
        }

        void OnDisable()
        {
            Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void SendDailySessionData()
        {
            string filepath = ComponentsConfigServices.DailySessionDataPath;

            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName
                (ComponentsConfigServices.SaveTypePath.DailySessionDataModel);

            BaseVisualizationDataManager<DailySessionDataModel> baseDataWorkflow =
                   new BaseVisualizationDataManager<DailySessionDataModel>();

            if (FolderList.Count > 0)
            {
                foreach (string fileName in FolderList)
                {
                    DailySessionDataModel dailySessionModel = baseDataWorkflow.GetData
                        (filepath,
                        fileName,
                       new DailySessionDataModel(),
                       new DailySessionDAL());

                    DateTime moment = DateTime.Now;
                    bool IsToday = dailySessionModel.TodayTime.Day == moment.Day;
                    if (IsToday)
                    {
                        dailySessionModel.TotalSessionTime += counterServices.TimerForGeneralSession;
                        dailySessionModel.SessionFrequency += 1;
                        baseDataWorkflow.SendData(WebApilink, dailySessionModel);
                        baseDataWorkflow.DeleteData(filepath, fileName, new DailySessionDAL());
                        baseDataWorkflow.SaveData(filepath, dailySessionModel, new DailySessionDAL());
                    }
                    else
                    {
                        baseDataWorkflow.SendData(WebApilink, dailySessionModel);
                        baseDataWorkflow.DeleteData
                            (filepath,
                            fileName,
                            new DailySessionDAL());
                    }


                }
            }
            else
            {
                DailySessionDataModel dailySessionDataModel = new DailySessionDataModel
                {
                    _id = playerId,
                    ProjectID = projectId,
                    CustomerID = CustomerId,
                    SessionFrequency = 1,
                    TotalSessionTime = counterServices.TimerForGeneralSession,
                    TodayTime = DateTime.Now
                };
                baseDataWorkflow.SendData(WebApilink, dailySessionDataModel);

                baseDataWorkflow.SaveData
                    (filepath,
                    dailySessionDataModel,
                    new DailySessionDAL());
            }

        }


        void SendLevelbaseSessionData(float sessionSeconds,
            string levelName,
            DateTime levelBaseGameSessionStart)
        {

            string filepath = ComponentsConfigServices.LevelBaseSessionDataPath;



            DateTime levelBaseGameSessionFinish = levelBaseGameSessionStart.AddSeconds(sessionSeconds);
            float minutes = sessionSeconds / 60;

            BaseVisualizationDataManager<LevelBaseSessionDataModel> baseDataWorkflow =
               new BaseVisualizationDataManager<LevelBaseSessionDataModel>();

            LevelBaseSessionDataModel dataModel = new LevelBaseSessionDataModel();
            LevelBaseSessionDAL dataAccess = new LevelBaseSessionDAL();

            string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            baseDataWorkflow.SaveData(filepath, new LevelBaseSessionDataModel
            {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                levelName = levelName,
                DifficultyLevel = ComponentsConfigServices.CurrentDifficultyLevel,
                SessionStartTime = levelBaseGameSessionStart,
                SessionFinishTime = levelBaseGameSessionFinish,
                SessionTimeMinute = minutes

            }, dataAccess);
        }

        void SendGeneralSessionData()
        {
            DateTime gameSessionEveryLoginFinish = counterServices.gameSessionEveryLoginStart.AddSeconds(counterServices.TimerForGeneralSession);
            float minutes = counterServices.TimerForGeneralSession / 60;

            string filepath = ComponentsConfigServices.GameSessionEveryLoginDataPath;




            BaseVisualizationDataManager<GameSessionEveryLoginDataModel> baseDataWorkflow =
                new BaseVisualizationDataManager<GameSessionEveryLoginDataModel>();

            GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel();
            GameSessionEveryLoginDAL dataAccess = new GameSessionEveryLoginDAL();

            string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            baseDataWorkflow.SaveData(filepath, new GameSessionEveryLoginDataModel
            {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                SessionStartTime = counterServices.gameSessionEveryLoginStart,
                SessionFinishTime = gameSessionEveryLoginFinish,
                SessionTimeMinute = minutes

            }, dataAccess);
        }


        void CheckGeneralDataAndSend()
        {
            BaseVisualizationDataManager<GameSessionEveryLoginDataModel> baseDataWorkflow =
            new BaseVisualizationDataManager<GameSessionEveryLoginDataModel>();

            GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel();
            GameSessionEveryLoginDAL modelDal = new GameSessionEveryLoginDAL();


            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.GameSessionEveryLoginDataModel);
            foreach (var item in FolderList)
            {
                dataModel = modelDal.Select(ComponentsConfigServices.GameSessionEveryLoginDataPath + item, dataModel);
                string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    modelDal.Delete(ComponentsConfigServices.GameSessionEveryLoginDataPath + item);
                }
            }


        }

        void CheckLevelBaseSessionDataAndSend()
        {
            BaseVisualizationDataManager<LevelBaseSessionDataModel> baseDataWorkflow =
            new BaseVisualizationDataManager<LevelBaseSessionDataModel>();

            LevelBaseSessionDataModel dataModel = new LevelBaseSessionDataModel();
            LevelBaseSessionDAL modelDal = new LevelBaseSessionDAL();


            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.LevelBaseSessionDataModel);
            foreach (var item in FolderList)
            {
                dataModel = modelDal.Select(ComponentsConfigServices.LevelBaseSessionDataPath + item, dataModel);
                string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    modelDal.Delete(ComponentsConfigServices.LevelBaseSessionDataPath + item);
                }
            }

        }


    }
}