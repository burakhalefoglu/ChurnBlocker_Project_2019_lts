using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.DataModel
{
    [Serializable]
    public class SuccessSaveInfo : IEntity
    {
        public bool IsSaved { get; set; }
    }
}
