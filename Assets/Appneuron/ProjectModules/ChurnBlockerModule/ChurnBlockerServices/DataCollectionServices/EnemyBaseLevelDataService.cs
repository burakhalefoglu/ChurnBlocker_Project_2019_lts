using AppneuronUnity.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager;
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
            EnemyBaseLevelController levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
            .GetComponent<EnemyBaseLevelController>();

            await new EnemybaseLevelManager().SendData(chartransform.position.x,
                chartransform.position.y,
                chartransform.position.z,
                isFail,
                charScores,
                totalPowerUsage,
                levelDatasManager.GetSceneName(),
                levelDatasManager.GetInTime());
        }

        public static async Task SendLevelData
            (int charScores,
            bool isFail,
            Transform chartransform)
        {
            EnemyBaseLevelController levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
          .GetComponent<EnemyBaseLevelController>();

            await new EnemybaseLevelManager().SendData(chartransform.position.x,
                chartransform.position.y,
                chartransform.position.z,
                isFail,
                charScores,
                0,
                levelDatasManager.GetSceneName(),
                levelDatasManager.GetInTime());
        }

        public static async Task SendLevelData
           (int charScores,
           bool isFail,
           int totalPowerUsage)
        {
            EnemyBaseLevelController levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
            .GetComponent<EnemyBaseLevelController>();

            await new EnemybaseLevelManager().SendData(0,
                0,
                0,
                isFail,
                charScores,
                totalPowerUsage,
                  levelDatasManager.GetSceneName(),
                levelDatasManager.GetInTime());
        }

        public static async Task SendLevelData
          (int charScores,
          bool isFail)
        {
            EnemyBaseLevelController levelDatasManager = GameObject.FindGameObjectWithTag("ChurnBlocker")
            .GetComponent<EnemyBaseLevelController>();

            await new EnemybaseLevelManager().SendData(0,
                0,
                0,
                isFail,
                charScores,
                0,
                levelDatasManager.GetSceneName(),
                levelDatasManager.GetInTime());
        }
    }
}