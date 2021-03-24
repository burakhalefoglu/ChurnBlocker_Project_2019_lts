using Assets.Appneuron.CoreServices.SaveDataServices.Abstract;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.DataAccess
{
    public interface ILevelBaseDieDal : IRepositoryService<LevelBaseDieDataModel>
    {
        
    }
}
