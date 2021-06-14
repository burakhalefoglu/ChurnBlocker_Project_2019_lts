using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.UnityManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.UnityListener
{
    public class AppneuronAdvListener : MonoBehaviour
    {
        private CounterServices counterServices;
        private AdvEventUnityManager advEventUnityManager;


        void Start()
        {
            advEventUnityManager = GameObject.FindGameObjectWithTag("ChurnBlocker").GetComponent<AdvEventUnityManager>();

            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            GameObject gameObject = this.gameObject;
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(async () =>
            {
                string levelName = counterServices.SceneName;
                float inMinutes = counterServices.LevelBaseGameTimer;
                await advEventUnityManager.SendAdvEventData(this.gameObject.tag,
                      levelName,
                      inMinutes);
            });
        }
    }
}
