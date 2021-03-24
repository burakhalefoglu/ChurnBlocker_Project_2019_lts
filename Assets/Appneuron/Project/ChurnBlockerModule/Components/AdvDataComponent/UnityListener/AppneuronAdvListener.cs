using Assets.Appneuron.Project.ChurnBlockerModule.ChildModules.DataVisualizationModule.Services.CounterServices;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityListener
{
    public class AppneuronAdvListener : MonoBehaviour
    {
       private IAdvEventUnityManager _advEventWorkflows;

        public AppneuronAdvListener(IAdvEventUnityManager advEventWorkflows)
        {
            _advEventWorkflows = advEventWorkflows;
        }

        CounterServices counterServices;
        
        void Start()
        {

            _advEventWorkflows.CheckAdvFileAndSendData();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            GameObject gameObject = this.gameObject;
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                string levelName = counterServices.SceneName;
                float inMinutes = counterServices.levelBaseGameTimer;
                _advEventWorkflows.SendAdvEventData(this.gameObject.tag,
                    levelName,
                    inMinutes);
            });
        }
    }
}
