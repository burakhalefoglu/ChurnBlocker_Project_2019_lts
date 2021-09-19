using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using System.Threading.Tasks;
using AppneuronUnity.ChurnBlockerModule.Components.SessionDataComponent.UnityManager;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.UnityManager
{
    public class SessionController : MonoBehaviour
    {
        private bool isNewLevel = true;
        private string levelName;

        private CounterServices counterServices;
        private LocalDataService localDataService;
        private SessionManager sessionManager;
        private async void Start()
        {
            sessionManager = new SessionManager();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            await LateStart(3);
        }
        async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await sessionManager.CheckGameSessionEveryLoginDataAndSend();
            await sessionManager.CheckLevelBaseSessionDataAndSend();
            localDataService.CheckLocalData += sessionManager.CheckGameSessionEveryLoginDataAndSend;
            localDataService.CheckLocalData += sessionManager.CheckLevelBaseSessionDataAndSend;
        }

        private async void OnApplicationQuit()
        {
            DateTime gameSessionEveryLoginFinish = counterServices.GameSessionEveryLoginStart
            .AddSeconds(counterServices.TimerForGeneralSession);
            float minutes = counterServices.TimerForGeneralSession / 60;

            await sessionManager.SendGameSessionEveryLoginData(counterServices.GameSessionEveryLoginStart,
                gameSessionEveryLoginFinish,
                minutes);

            localDataService.CheckLocalData -= sessionManager.CheckGameSessionEveryLoginDataAndSend;
            localDataService.CheckLocalData -= sessionManager.CheckLevelBaseSessionDataAndSend;
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
                await sessionManager.SendLevelbaseSessionData(counterServices.LevelBaseGameTimer, levelName, counterServices.LevelBaseGameSessionStart);
                levelName = scene.name;

            }
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

    }
}