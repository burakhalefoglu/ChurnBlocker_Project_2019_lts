

using Assets.Appneuron.DataAccessBase.Abstract;
using Assets.Appneuron.DataAccessBase.Concrete.Base;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataModel;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataAccess
{
    class DailySessionDAL : DataAccessBase<DailySessionDataModel>, IModelDal<DailySessionDataModel>
    {
    }
}
