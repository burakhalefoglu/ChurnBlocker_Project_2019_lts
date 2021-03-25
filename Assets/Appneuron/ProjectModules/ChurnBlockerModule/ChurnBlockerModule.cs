
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.ConfigServices;
using UnityEngine;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule
{
    public class ChurnBlockerModule : MonoBehaviour
    {


        private void Awake()
        {
            ComponentsConfigServices.CreateFileVisualDataDirectories();
        }
     
    }
}
