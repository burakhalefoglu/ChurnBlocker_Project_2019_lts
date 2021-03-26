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
        public static void GetLevelDatas
            (int charScores,
            bool isDead,
            int totalPowerUsage,
            Transform chartransform)
        {

            LevelDatasManager levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
                .GetComponent<LevelDatasManager>();
            levelDatasManager.SendData(chartransform.position.x,
                chartransform.position.y,
                chartransform.position.z,
                isDead,
                charScores,
                totalPowerUsage);
        }
    }
}