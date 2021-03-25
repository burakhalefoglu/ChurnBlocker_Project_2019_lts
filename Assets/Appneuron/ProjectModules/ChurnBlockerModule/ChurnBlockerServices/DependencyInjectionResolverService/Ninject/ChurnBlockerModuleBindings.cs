using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.BinarySaving;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess.BinarySaving;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.DataAccess.BinarySaving;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.DataAccess.BinarySaving;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataAccess.BinarySaving;


namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.DependencyInjectionResolverService.Ninject
{

    public class ChurnBlockerModuleBindings : NinjectModule
    {
        public override void Load()
        {

            Bind<IAdvEventDal>().To<BSAdvEventDal>().InSingletonScope();
            Bind<IBuyingEventDal>().To<BSBuyingEventDal>().InSingletonScope();
            Bind<ISuccessSaveInfoDal>().To<BSSuccessSaveInfoDal>().InSingletonScope();
            Bind<IGeneralDataDal>().To<BSGeneralDataDal>().InSingletonScope();
            Bind<ILevelBaseDieDal>().To<BSLevelBaseDieDal>().InSingletonScope();
            Bind<IEveryLoginLevelDal>().To<BSEveryLoginLevelDal>().InSingletonScope();
            Bind<IDailySessionDal>().To<BSDailySessionDal>().InSingletonScope();
            Bind<IGameSessionEveryLoginDal>().To<BSGameSessionEveryLoginDal>().InSingletonScope();
            Bind<ILevelBaseSessionDal>().To<BSLevelBaseSessionDal>().InSingletonScope();

        }
    }
}
