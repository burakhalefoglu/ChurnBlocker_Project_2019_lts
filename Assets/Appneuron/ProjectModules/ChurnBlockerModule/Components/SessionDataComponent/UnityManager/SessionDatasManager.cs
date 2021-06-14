using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ninject;
using System.Reflection;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using Appneuron.Models;
using Appneuron.Services;
using System.Threading.Tasks;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.UnityManager
{
    public class SessionDatasManager : MonoBehaviour
    {

        private string playerId;
        private string projectId;
        private string customerId;
        private bool isNewLevel = true;
        private string levelName;
        private string LevelBaseSessionDatasRequestPath;
        private string DailySessionDatasRequestPath;
        private string GameSessionEveryLoginDataRequestPath;


        private IDailySessionDal _dailySessionDal;
        private IGameSessionEveryLoginDal _gameSessionEveryLoginDal;
        private ILevelBaseSessionDal _levelBaseSessionDal;
        private IRestClientServices _restClientServices;
        private ICryptoServices _cryptoServices;


        private CounterServices counterServices;
        private DifficultySingletonModel difficultySingletonModel;
        private LocalDataService localDataService;



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

        private async void Start()
        {

            IdUnityManager idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            LevelBaseSessionDatasRequestPath = WebApiConfigService.ClientWebApiLink + WebApiConfigService.LevelBaseSessionDatasRequestName;
            DailySessionDatasRequestPath = WebApiConfigService.ClientWebApiLink + WebApiConfigService.DailySessionDatasRequestName;
            GameSessionEveryLoginDataRequestPath = WebApiConfigService.ClientWebApiLink + WebApiConfigService.GameSessionEveryLoginDatasRequestName;


            difficultySingletonModel = DifficultySingletonModel.Instance;

            playerId = await idUnityManager.GetPlayerID();
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
            await SendGameSessionEveryLoginData();
            await SendDailySessionData();
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






        private async Task SendDailySessionData()
        {
            string filepath = ComponentsConfigService.DailySessionDataPath;

            List<string> FolderList = ComponentsConfigService.GetVisualDataFilesName
                (ComponentsConfigService.SaveTypePath.DailySessionDataModel);

            if (FolderList.Count > 0)
            {
                foreach (string fileName in FolderList)
                {
                    var dailySessionModel = await _dailySessionDal.SelectAsync(filepath + fileName);
                    DateTime moment = DateTime.Now;
                    bool IsToday = dailySessionModel.TodayTime.Day == moment.Day;
                    if (IsToday)
                    {
                        dailySessionModel.TotalSessionTime += counterServices.TimerForGeneralSession;
                        dailySessionModel.SessionFrequency += 1;

                        await _restClientServices.PostAsync<System.Object>(DailySessionDatasRequestPath, dailySessionModel);
                        await _dailySessionDal.DeleteAsync(filepath + fileName);
                        await _dailySessionDal.InsertAsync(filepath + fileName, dailySessionModel);
                    }
                    else
                    {
                        var result = await _restClientServices.PostAsync<System.Object>(DailySessionDatasRequestPath, dailySessionModel);
                        if (result.Success)
                            await _dailySessionDal.DeleteAsync(filepath + fileName);
                    }


                }
            }
            else
            {
                DailySessionDataModel dailySessionDataModel = new DailySessionDataModel
                {
                    ClientId = playerId,
                    ProjectID = projectId,
                    CustomerID = customerId,
                    SessionFrequency = 1,
                    TotalSessionTime = counterServices.TimerForGeneralSession,
                    TodayTime = DateTime.Now
                };
                await _restClientServices.PostAsync<System.Object>(DailySessionDatasRequestPath, dailySessionDataModel);
                string fileName = _cryptoServices.GenerateStringName(6);
                await _dailySessionDal.InsertAsync(filepath + fileName, dailySessionDataModel);

            }

        }









        private async Task SendLevelbaseSessionData(float sessionSeconds,
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

            var result = await _restClientServices.PostAsync<System.Object>(LevelBaseSessionDatasRequestPath, dataModel);

            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            await _levelBaseSessionDal.InsertAsync(filepath + fileName, dataModel);
        }

        private async Task CheckLevelBaseSessionDataAndSend()
        {

            List<string> FolderList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService.SaveTypePath.LevelBaseSessionDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _levelBaseSessionDal.SelectAsync(ComponentsConfigService.LevelBaseSessionDataPath + fileName);
                var result = await _restClientServices.PostAsync<System.Object>(LevelBaseSessionDatasRequestPath, dataModel);
                if (result.Success)
                {
                    await _levelBaseSessionDal.DeleteAsync(ComponentsConfigService.LevelBaseSessionDataPath + fileName);
                }
            }

        }











        private async Task SendGameSessionEveryLoginData()
        {
            DateTime gameSessionEveryLoginFinish = counterServices.GameSessionEveryLoginStart.AddSeconds(counterServices.TimerForGeneralSession);
            float minutes = counterServices.TimerForGeneralSession / 60;
            string filepath = ComponentsConfigService.GameSessionEveryLoginDataPath;

            GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel
            {

                ClientId = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                SessionStartTime = counterServices.GameSessionEveryLoginStart,
                SessionFinishTime = gameSessionEveryLoginFinish,
                SessionTimeMinute = minutes

            };

            var result = await _restClientServices.PostAsync<System.Object>(GameSessionEveryLoginDataRequestPath, dataModel);

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
            List<string> FolderList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService.SaveTypePath.GameSessionEveryLoginDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _gameSessionEveryLoginDal.SelectAsync(ComponentsConfigService.GameSessionEveryLoginDataPath + fileName);
                var result = await _restClientServices.PostAsync<System.Object>(GameSessionEveryLoginDataRequestPath, dataModel);
                if (result.Success)
                {
                    await _gameSessionEveryLoginDal.DeleteAsync(ComponentsConfigService.GameSessionEveryLoginDataPath + fileName);
                }
            }


        }




    }
}