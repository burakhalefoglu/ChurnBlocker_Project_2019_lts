using Assets.Appneuron.Project.ChurnBlockerModule.ChildModules.DataVisualizationModule.Services.CounterServices;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.UnityListener
{
    public class BuyingEventListener: MonoBehaviour
    {

        private readonly IBuyingEventDataManager _buyingEventDataManager;
        CounterServices counterServices;
        public BuyingEventListener(IBuyingEventDataManager buyingEventDataManager)
        {
            _buyingEventDataManager = buyingEventDataManager;
        }


        void Start()
        {
            _buyingEventDataManager.CheckAdvFileAndSendData();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            GameObject gameObject = this.gameObject;
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                string levelName = counterServices.SceneName;
                float inMinutes = counterServices.levelBaseGameTimer;
                _buyingEventDataManager.SendAdvEventData(this.gameObject.tag,
                    levelName,
                    inMinutes);
            });
        }
    }
}
