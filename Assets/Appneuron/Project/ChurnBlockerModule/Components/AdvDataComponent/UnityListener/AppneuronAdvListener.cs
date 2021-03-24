using Assets.Appneuron.Project.ChurnBlockerModule.ChildModules.DataVisualizationModule.Services.CounterServices;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityWorkflow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityListener
{
    public class AppneuronAdvListener : MonoBehaviour
    {
        AdvEventManager advEventWorkflows = new AdvEventManager();
        CounterServices counterServices;
        void Start()
        {
            advEventWorkflows.CheckAdvFileAndSendData();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            GameObject gameObject = this.gameObject;
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                string levelName = counterServices.SceneName;
                float inMinutes = counterServices.levelBaseGameTimer;
                advEventWorkflows.SendAdvEventData(this.gameObject.tag,
                    levelName,
                    inMinutes);
            });
        }
    }
}
