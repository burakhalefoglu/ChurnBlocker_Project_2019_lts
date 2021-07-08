using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.DataCollectionServices
{
    public static class EnemyBaseLevelDataService
    {
        public static async Task SendLevelData
            (int charScores,
            bool isFail,
            int totalPowerUsage,
            Transform chartransform)
        {

            EnemyBaseLevelDatasManager levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
                .GetComponent<EnemyBaseLevelDatasManager>();
            await levelDatasManager.SendData(chartransform.position.x,
                chartransform.position.y,
                chartransform.position.z,
                isFail,
                charScores,
                totalPowerUsage);
        }

        public static async Task SendLevelData
            (int charScores,
            bool isFail,
            Transform chartransform)
        {

            EnemyBaseLevelDatasManager levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
                .GetComponent<EnemyBaseLevelDatasManager>();
            await levelDatasManager.SendData(chartransform.position.x,
                chartransform.position.y,
                chartransform.position.z,
                isFail,
                charScores,
                0);
        }

        public static async Task SendLevelData
           (int charScores,
           bool isFail,
           int totalPowerUsage)
        {

            EnemyBaseLevelDatasManager levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
                .GetComponent<EnemyBaseLevelDatasManager>();
            await levelDatasManager.SendData(0,
                0,
                0,
                isFail,
                charScores,
                totalPowerUsage);
        }

        public static async Task SendLevelData
          (int charScores,
          bool isFail)
        {

            EnemyBaseLevelDatasManager levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
                .GetComponent<EnemyBaseLevelDatasManager>();
            await levelDatasManager.SendData(0,
                0,
                0,
                isFail,
                charScores,
                0);
        }
    }
}