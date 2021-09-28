using AppneuronUnity.Core.IoC.Zenject;
using Zenject;

namespace Assets.Appneuron.ProjectModules.ZenjectInstaller
{
    public class CoreBindingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            CoreBindingService.Install(Container);
        }
    }
}