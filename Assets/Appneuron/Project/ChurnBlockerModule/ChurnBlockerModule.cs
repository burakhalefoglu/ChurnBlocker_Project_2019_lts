
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using UnityEngine;

namespace Assets.Appneuron.Project.ChurnBlockerModule
{
    public class ChurnBlockerModule : MonoBehaviour
    {


        private void Awake()
        {
            ComponentsConfigServices.CreateFileVisualDataDirectories();
        }
     
    }
}
