using Ninject.Modules;
using Assets.Appneuron.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.CoreServices.CryptoServices.Concrete.WithEffortless;
using Assets.Appneuron.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.CoreServices.RestClientServices.Concrete.RestSharp;
using Assets.Appneuron.CoreServices.SaveDataServices.Concrete.BinaryData;
using Assets.Appneuron.CoreServices.SaveDataServices.Abstract;

namespace Assets.Appneuron.CoreServices.IocContainerServices.Ninject
{
    public class NinjectServices : NinjectModule
    {
        public override void Load()
        {
            Bind<ICryptoServices>().To<EfforlessCryptoServices>();
            Bind<IDataSaveServices>().To<BinaryDataSaveServices>();
            Bind<IRestClientServices>().To<RestSharpServices>();

        }
    }
}