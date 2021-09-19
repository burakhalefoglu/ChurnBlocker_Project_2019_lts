using System;
using System.Threading.Tasks;
using UnityEngine;
using Appneuron.Models;
using AppneuronUnity.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager
{
    public class BuyingEventController : MonoBehaviour
    {
        private LocalDataService localDataService;
        private BuyingEventManager buyingEventManager;
        private async void Start()
        {
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            buyingEventManager = new BuyingEventManager();
            await LateStart(3);
        }

        async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await buyingEventManager.CheckAdvFileAndSendData();

            localDataService.CheckLocalData += buyingEventManager.CheckAdvFileAndSendData;
        }

        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= buyingEventManager.CheckAdvFileAndSendData;

        }


      

    }
}
