using AppneuronUnity.ProductModules.ChurnBlockerModule.IoC.Zenject;
using UnityEngine;
using Zenject;

namespace Assets.Appneuron.ProjectModules.ZenjectInstaller
{
    public class ChurnBlockerBindingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ChurnBlockerBindingService.Install(Container);
        }
    }
}