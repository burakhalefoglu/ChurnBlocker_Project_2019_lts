using Assets.Appneuron.DataAccessBase.Abstract;
using Assets.Appneuron.DataAccessBase.Concrete.Base;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess
{
    public class BuyingEventDAL : DataAccessBase<BuyingEventDataModel>, IModelDal<BuyingEventDataModel>
    {
    }
}

