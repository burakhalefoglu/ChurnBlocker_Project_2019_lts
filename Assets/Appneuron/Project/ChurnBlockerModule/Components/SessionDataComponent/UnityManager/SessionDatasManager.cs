
using Assets.Appneuron.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Project.ChurnBlockerModule.ChildModules.DataVisualizationModule.Services.CounterServices;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.UnityWorkflow
{
    public class SessionDatasManager : MonoBehaviour
    {
        private readonly IDailySessionDal _dailySessionDal;
        private readonly IGameSessionEveryLoginDal _gameSessionEveryLoginDal;
        private readonly ILevelBaseSessionDal _levelBaseSessionDal;
        private readonly ICryptoServices _cryptoServices;
        private readonly IRestClientServices _restClientServices;

        public SessionDatasManager(IDailySessionDal dailySessionDal,
            IGameSessionEveryLoginDal gameSessionEveryLoginDal,
            ILevelBaseSessionDal levelBaseSessionDal,
            ICryptoServices cryptoServices,
            IRestClientServices restClientServices)
        {
            _dailySessionDal = dailySessionDal;
            _gameSessionEveryLoginDal = gameSessionEveryLoginDal;
            _levelBaseSessionDal = levelBaseSessionDal;
            _cryptoServices = cryptoServices;
            _restClientServices = restClientServices;
        }



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

            if (FolderList.Count > 0)
            {
                foreach (string fileName in FolderList)
                {
                    var dailySessionModel = _dailySessionDal.Select(filepath + fileName);
                    DateTime moment = DateTime.Now;
                    bool IsToday = dailySessionModel.TodayTime.Day == moment.Day;
                    if (IsToday)
                    {
                        dailySessionModel.TotalSessionTime += counterServices.TimerForGeneralSession;
                        dailySessionModel.SessionFrequency += 1;
                        _restClientServices.Post(WebApilink, dailySessionModel);
                       _dailySessionDal.Delete(filepath + fileName);
                        _dailySessionDal.Insert(filepath + fileName, dailySessionModel);
                    }
                    else
                    {
                        _restClientServices.Post(WebApilink, dailySessionModel);
                        _dailySessionDal.Delete(filepath + fileName);
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
                _restClientServices.Post(WebApilink, dailySessionDataModel);
                string fileName = _cryptoServices.GenerateStringName(6);
                _dailySessionDal.Insert(filepath + fileName, dailySessionDataModel);

            }

        }


        void SendLevelbaseSessionData(float sessionSeconds,
            string levelName,
            DateTime levelBaseGameSessionStart)
        {

            string filepath = ComponentsConfigServices.LevelBaseSessionDataPath;
            DateTime levelBaseGameSessionFinish = levelBaseGameSessionStart.AddSeconds(sessionSeconds);
            float minutes = sessionSeconds / 60;

            LevelBaseSessionDataModel dataModel = new LevelBaseSessionDataModel {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                levelName = levelName,
                DifficultyLevel = ComponentsConfigServices.CurrentDifficultyLevel,
                SessionStartTime = levelBaseGameSessionStart,
                SessionFinishTime = levelBaseGameSessionFinish,
                SessionTimeMinute = minutes
            };

            string statuseCode =_restClientServices.Post(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            _levelBaseSessionDal.Insert(filepath + fileName, dataModel);
        }



        void SendGeneralSessionData()
        {
            DateTime gameSessionEveryLoginFinish = counterServices.gameSessionEveryLoginStart.AddSeconds(counterServices.TimerForGeneralSession);
            float minutes = counterServices.TimerForGeneralSession / 60;
            string filepath = ComponentsConfigServices.GameSessionEveryLoginDataPath;

            GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                SessionStartTime = counterServices.gameSessionEveryLoginStart,
                SessionFinishTime = gameSessionEveryLoginFinish,
                SessionTimeMinute = minutes

            };

            string statuseCode =_restClientServices.Post(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            _gameSessionEveryLoginDal.Insert(filepath + fileName, dataModel);
        }


        void CheckGeneralDataAndSend()
        {
            
            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.GameSessionEveryLoginDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = _gameSessionEveryLoginDal.Select(ComponentsConfigServices.GameSessionEveryLoginDataPath + fileName);
                string statuseCode =_restClientServices.Post(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    _gameSessionEveryLoginDal.Delete(ComponentsConfigServices.GameSessionEveryLoginDataPath + fileName);
                }
            }


        }

        void CheckLevelBaseSessionDataAndSend()
        {
            
            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.LevelBaseSessionDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = _levelBaseSessionDal.Select(ComponentsConfigServices.LevelBaseSessionDataPath + fileName);
                string statuseCode =_restClientServices.Post(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    _levelBaseSessionDal.Delete(ComponentsConfigServices.LevelBaseSessionDataPath + fileName);
                }
            }

        }


    }
}