using Assets.Appneuron.CoreServices.SaveDataServices.Abstract;
using Assets.Appneuron.DataModelBase.Abstract;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.DataAccess
{
    public interface IAdvEventDal:IRepositoryService<AdvEventDataModel>
    {

    }
}
