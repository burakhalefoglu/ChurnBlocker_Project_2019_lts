using Assets.Appneuron.CoreServices.SaveDataServices.Abstract;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.GeneralDataComponent.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.GeneralDataComponent.DataAccess
{
    public interface ISuccessSaveInfoDal : IRepositoryService<SuccessSaveInfo>
    {
    }
}
