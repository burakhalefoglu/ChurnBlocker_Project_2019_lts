
using UnityEngine;
using Appneuron.Services;
using Appneuron.Models;
using Appneuron.Core.Components.HardwareIndormationCompenent;
using System.Threading.Tasks;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule
{
    public class ChurnBlockerModule : MonoBehaviour
    {
        private void Awake()
        {
            ComponentsConfigService.CreateFileLocalDataDirectories();
        }

        private async void Start()
        {
             await new GeneralInformationManager().SendGeneralInformation();
        }

    }
}
