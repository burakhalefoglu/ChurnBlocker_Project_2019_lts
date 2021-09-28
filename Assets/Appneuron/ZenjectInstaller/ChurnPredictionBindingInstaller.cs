using AppneuronUnity.ProductModules.ChurnPredictionModule.IoC.Zenject;
using UnityEngine;
using Zenject;

namespace Assets.Appneuron.ProjectModules.ZenjectInstaller
{
    public class ChurnPredictionBindingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ChurnPredictionBindingService.Install(Container);
        }
    }
}