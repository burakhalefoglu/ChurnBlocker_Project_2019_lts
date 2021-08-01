
using UnityEngine;
using Appneuron.Services;
using Appneuron.Models;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule
{
    public class ChurnBlockerModule : MonoBehaviour
    {

        DifficultySingletonModel difficultySingletonModel;
        private void Awake()
        {
            ComponentsConfigService.CreateFileLocalDataDirectories();
            difficultySingletonModel = DifficultySingletonModel.Instance;
        }

        private async void Start()
        {
            await MessageBrokerAdminHelper.SetPartitionCountAsync();

        }

    }
}
