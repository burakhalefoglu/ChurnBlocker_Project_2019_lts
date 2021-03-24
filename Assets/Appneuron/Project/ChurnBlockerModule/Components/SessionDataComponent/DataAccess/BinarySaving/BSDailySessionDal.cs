
using Assets.Appneuron.CoreServices.SaveDataServices.Concrete.BinaryDataBase;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataAccess.BinarySaving
{
    public class BSDailySessionDal : BinaryTypeRepositoryBase<DailySessionDataModel>, IDailySessionDal
    {
    }
}
