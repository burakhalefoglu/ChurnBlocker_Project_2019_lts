
using Assets.Appneuron.DataAccessBase.Abstract;
using Assets.Appneuron.DataAccessBase.Concrete.Base;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.DataModel;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.DataAccess
{
    public class AdvEventDAL : DataAccessBase<AdvEventDataModel>, IModelDal<AdvEventDataModel>
    {
    }
}
