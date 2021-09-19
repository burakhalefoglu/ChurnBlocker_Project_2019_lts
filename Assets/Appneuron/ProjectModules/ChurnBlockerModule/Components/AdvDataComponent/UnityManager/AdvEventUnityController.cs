using System;
using UnityEngine;
using System.Threading.Tasks;
using AppneuronUnity.ChurnBlockerModule.Components.AdvDataComponent.UnityManager;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.UnityManager
{

    public class AdvEventUnityController : MonoBehaviour
    {
        private LocalDataService localDataService;
        private AdvEventUnityManager advEventUnityManager;
        private async void Start()
        {
            advEventUnityManager = new AdvEventUnityManager();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            await LateStart(3);

        }
        async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await advEventUnityManager.CheckAdvFileAndSendData();
            localDataService.CheckLocalData += advEventUnityManager.CheckAdvFileAndSendData;
        }

        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= advEventUnityManager.CheckAdvFileAndSendData;

        }
        
    }
}