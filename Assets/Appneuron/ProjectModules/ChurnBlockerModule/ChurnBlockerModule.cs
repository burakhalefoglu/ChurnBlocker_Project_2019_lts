
using UnityEngine;
using Appneuron.Services;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.DataCollectionServices;
using Appneuron.Models;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule
{
    public class ChurnBlockerModule : MonoBehaviour
    {

        DifficultySingletonModel difficultySingletonModel;
        private void Awake()
        {
            ComponentsConfigService.CreateFileVisualDataDirectories();
            difficultySingletonModel = DifficultySingletonModel.Instance;
        }

    }
}
