using Assets.Appneuron.DataModelBase.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.GeneralDataComponent.DataModel
{
    [Serializable]
    public class SuccessSaveInfo : IDataModel
    {
        public bool IsSaved { get; set; }
    }
}
