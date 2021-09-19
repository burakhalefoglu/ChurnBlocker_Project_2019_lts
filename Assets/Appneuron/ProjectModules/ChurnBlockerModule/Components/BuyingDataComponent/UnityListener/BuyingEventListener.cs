using AppneuronUnity.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager;
using AppneuronUnity.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.UnityListener
{
    public class BuyingEventListener : MonoBehaviour
    {
        private CounterServices counterServices;
        private IdUnityManager idUnityManager;
        private BuyingEventManager buyingEventManager;

        void Start()
        {
            buyingEventManager = new BuyingEventManager();
            idUnityManager = new IdUnityManager();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            Button button = this.gameObject.GetComponent<Button>();
            button.onClick.AddListener(async () =>
            {
                await buyingEventManager.SendAdvEventData(this.gameObject.tag,
                    counterServices.SceneName,
                    counterServices.LevelBaseGameTimer,
                    idUnityManager.GetPlayerID());
            });
        }
    }
}
