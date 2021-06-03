using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.UnityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.DataCollectionServices
{
    public static class LevelDataCollectionService
    {
        //overloading ile farklı senaryolar için fonksiyonlar yazılacak....
        public static async Task GetLevelDatas
            (int charScores,
            bool isDead,
            int totalPowerUsage,
            Transform chartransform)
        {

            LevelDatasManager levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
                .GetComponent<LevelDatasManager>();
            await levelDatasManager.SendData(chartransform.position.x,
                chartransform.position.y,
                chartransform.position.z,
                isDead,
                charScores,
                totalPowerUsage);
        }
        public static void GetLevelDatas
           (int charScores,
           bool isDead,
           int totalPowerUsage
          )
        {
            LevelDatasManager levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
                    .GetComponent<LevelDatasManager>();

        }
    }
}