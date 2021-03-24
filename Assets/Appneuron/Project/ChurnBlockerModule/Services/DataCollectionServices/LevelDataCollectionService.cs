using Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.UnityWorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Services.DataCollectionServices
{
    public static class LevelDataCollectionService
    {
        public static void GetLevelDatas
            (int CharScores,
            bool isDead,
            int totalPowerUsage,
            Transform Chartransform)
        {

            LevelDatasManager levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker").GetComponent<LevelDatasManager>();
            levelDatasManager.SendData(Chartransform.position.x,
                Chartransform.position.y,
                Chartransform.position.z,
                isDead,
                CharScores,
                totalPowerUsage);
        }
    }
}