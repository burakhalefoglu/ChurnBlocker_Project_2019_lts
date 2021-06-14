using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.UnityListener
{
    public class BuyingEventListener : MonoBehaviour
    {
        private CounterServices counterServices;
        private BuyingEventDataManager _buyingEventDataManager;

        void Start()
        {
            _buyingEventDataManager = GameObject.FindGameObjectWithTag("ChurnBlocker").GetComponent<BuyingEventDataManager>();
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            GameObject gameObject = this.gameObject;
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(async () =>
            {
                string levelName = counterServices.SceneName;
                float inMinutes = counterServices.LevelBaseGameTimer;
                await _buyingEventDataManager.SendAdvEventData(this.gameObject.tag,
                    levelName,
                    inMinutes);
            });
        }
    }
}
