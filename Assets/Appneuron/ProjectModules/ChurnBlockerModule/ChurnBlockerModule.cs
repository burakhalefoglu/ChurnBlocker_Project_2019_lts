
using Appneuron;
using UnityEngine;
using Appneuron.Services;

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
