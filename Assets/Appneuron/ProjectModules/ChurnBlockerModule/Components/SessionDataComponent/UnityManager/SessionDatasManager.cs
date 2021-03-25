using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ninject;
using System.Reflection;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.ConfigServices;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.UnityManager
{
    public class SessionDatasManager : MonoBehaviour
    {

        private IDailySessionDal _dailySessionDal;
        private IGameSessionEveryLoginDal _gameSessionEveryLoginDal;
        private ILevelBaseSessionDal _levelBaseSessionDal;
        private IRestClientServices _restClientServices;
        private ICryptoServices _cryptoServices;
         

        private string playerId;
        private string projectId;
        private string customerId;
        private string webApilink;
        private bool isNewLevel = true;
        private string levelName;

        private CounterServices counterServices;

        private void Awake()
        {
            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                _dailySessionDal = kernel.Get<IDailySessionDal>();
                _gameSessionEveryLoginDal = kernel.Get<IGameSessionEveryLoginDal>();
                _levelBaseSessionDal = kernel.Get<ILevelBaseSessionDal>();
                _restClientServices = kernel.Get<IRestClientServices>();
                _cryptoServices = kernel.Get<ICryptoServices>();
            }
        }

        void Start()
        {

            IdUnityManager idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();

            playerId = idUnityManager.GetPlayerID();
            projectId = ChurnBlockerConfigServices.GetProjectID();
            customerId = ChurnBlockerConfigServices.GetCustomerID();
            webApilink = ChurnBlockerConfigServices.GetWebApiLink();

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
            if (isNewLevel)
            {
                levelName = scene.name;
                isNewLevel = false;
            }
            else
            {
                SendLevelbaseSessionData(counterServices.LevelBaseGameTimer, levelName, counterServices.LevelBaseGameSessionStart);
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
                        _restClientServices.Post(webApilink, dailySessionModel);
                        _dailySessionDal.Delete(filepath + fileName);
                        _dailySessionDal.Insert(filepath + fileName, dailySessionModel);
                    }
                    else
                    {
                        _restClientServices.Post(webApilink, dailySessionModel);
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
                    CustomerID = customerId,
                    SessionFrequency = 1,
                    TotalSessionTime = counterServices.TimerForGeneralSession,
                    TodayTime = DateTime.Now
                };
                _restClientServices.Post(webApilink, dailySessionDataModel);
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

            LevelBaseSessionDataModel dataModel = new LevelBaseSessionDataModel
            {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                levelName = levelName,
                DifficultyLevel = ComponentsConfigServices.CurrentDifficultyLevel,
                SessionStartTime = levelBaseGameSessionStart,
                SessionFinishTime = levelBaseGameSessionFinish,
                SessionTimeMinute = minutes
            };

            var result = _restClientServices.Post(webApilink, dataModel);

            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            _levelBaseSessionDal.Insert(filepath + fileName, dataModel);
        }



        void SendGeneralSessionData()
        {
            DateTime gameSessionEveryLoginFinish = counterServices.GameSessionEveryLoginStart.AddSeconds(counterServices.TimerForGeneralSession);
            float minutes = counterServices.TimerForGeneralSession / 60;
            string filepath = ComponentsConfigServices.GameSessionEveryLoginDataPath;

            GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel
            {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                SessionStartTime = counterServices.GameSessionEveryLoginStart,
                SessionFinishTime = gameSessionEveryLoginFinish,
                SessionTimeMinute = minutes

            };

            var result = _restClientServices.Post(webApilink, dataModel);

            if (result.Success)
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
                var result = _restClientServices.Post(webApilink, dataModel);
                if (result.Success)
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
                var result = _restClientServices.Post(webApilink, dataModel);
                if (result.Success)
                {
                    _levelBaseSessionDal.Delete(ComponentsConfigServices.LevelBaseSessionDataPath + fileName);
                }
            }

        }


    }
}