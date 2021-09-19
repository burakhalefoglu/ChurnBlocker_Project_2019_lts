using System;
using UnityEngine;
using System.Threading.Tasks;
using AppneuronUnity.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager;
using UnityEngine.SceneManagement;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager
{
    public class EnemyBaseLevelController : MonoBehaviour
    {
        private LocalDataService localDataService;
        private EnemybaseLevelManager enemybaseLevelManager;
        CounterServices counterService;
        private async void Start()
        {
            enemybaseLevelManager = new EnemybaseLevelManager();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            counterService = GameObject.FindGameObjectWithTag("ChurnBlocker").GetComponent<CounterServices>();
            await LateStart(3);
        }

        public string GetSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        public int GetInTime()
        {
            return (int)(counterService.LevelBaseGameTimer);
        }

        async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend();
            await enemybaseLevelManager.CheckLevelbaseDieAndSend();
            localDataService.CheckLocalData += enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData += enemybaseLevelManager.CheckLevelbaseDieAndSend;
        }

        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData -= enemybaseLevelManager.CheckLevelbaseDieAndSend;
        }        
    }
}