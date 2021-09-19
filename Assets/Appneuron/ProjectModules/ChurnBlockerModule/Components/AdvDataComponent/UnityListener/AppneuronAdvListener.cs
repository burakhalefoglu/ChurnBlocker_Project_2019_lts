using AppneuronUnity.ChurnBlockerModule.Components.AdvDataComponent.UnityManager;
using AppneuronUnity.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.UnityListener
{
    public class AppneuronAdvListener : MonoBehaviour
    {
        private CounterServices counterServices;
        private IdUnityManager idUnityManager;
        private AdvEventUnityManager advEventUnityManager;


        void Start()
        {
            idUnityManager = new IdUnityManager();
            advEventUnityManager = new AdvEventUnityManager();

            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            GameObject gameObject = this.gameObject;
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(async () =>
            {
                await advEventUnityManager.SendAdvEventData(this.gameObject.tag,
                counterServices.SceneName,
                counterServices.LevelBaseGameTimer,
                idUnityManager.GetPlayerID());
            });
        }


    }
}
