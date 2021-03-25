
using Assets.Appneuron.Core.CoreServices.SaveDataServices.Concrete.BinaryDataBase;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataAccess.BinarySaving
{
    public class BSDailySessionDal : BinaryTypeRepositoryBase<DailySessionDataModel>, IDailySessionDal
    {
    }
}
