using Assets.Appneuron.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.CoreServices.CryptoServices.Concrete.WithEffortless;
using Assets.Appneuron.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.CoreServices.RestClientServices.Concrete.RestSharp;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.BinarySaving;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityManager;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityWorkflow;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess.BinarySaving;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.GeneralDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.GeneralDataComponent.DataAccess.BinarySaving;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.DataAccess.BinarySaving;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataAccess.BinarySaving;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.CoreServices.IocContainerServices.Autofac
{
    public class AutofacCoreServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfforlessCryptoServices>().As<ICryptoServices>().SingleInstance();
            builder.RegisterType<RestSharpServices>().As<IRestClientServices>().SingleInstance();

            builder.RegisterType<BSAdvEventDal>().As<IAdvEventDal>().SingleInstance();
            builder.RegisterType<AdvEventUnityManager>().As<IAdvEventUnityManager>().SingleInstance();

            builder.RegisterType<BSBuyingEventDal>().As<IBuyingEventDal>().SingleInstance();
            builder.RegisterType<BuyingEventDataManager>().As<IBuyingEventDataManager>().SingleInstance();

            builder.RegisterType<BSSuccessSaveInfoDal>().As<ISuccessSaveInfoDal>().SingleInstance();
            builder.RegisterType<BSGeneralDataDal>().As<IGeneralDataDal>().SingleInstance();

            builder.RegisterType<BSLevelBaseDieDal>().As<ILevelBaseDieDal>().SingleInstance();
            builder.RegisterType<BSEveryLoginLevelDal>().As<IEveryLoginLevelDal>().SingleInstance();

            builder.RegisterType<BSDailySessionDal>().As<IDailySessionDal>().SingleInstance();
            builder.RegisterType<BSGameSessionEveryLoginDal>().As<IGameSessionEveryLoginDal>().SingleInstance();
            builder.RegisterType<BSLevelBaseSessionDal>().As<ILevelBaseSessionDal>().SingleInstance();




            //var assembly = System.Reflection.Assembly.GetExecutingAssembly();


            //builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
            //    .EnableInterfaceInterceptors(new ProxyGenerationOptions()
            //    {
            //        Selector = new AspectInterceptorSelector()
            //    }).SingleInstance();


        }

    }
}
