
using Assets.Appneuron.CoreServices.SaveDataServices.Concrete.BinaryDataBase;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess.BinarySaving
{
    public class BSBuyingEventDal : BinaryTypeRepositoryBase<BuyingEventDataModel>, IBuyingEventDal
    {
    }
}

