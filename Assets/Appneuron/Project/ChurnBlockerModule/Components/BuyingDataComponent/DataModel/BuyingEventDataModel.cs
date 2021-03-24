using Assets.Appneuron.DataModelBase.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataModel
{
    [Serializable]
    public class BuyingEventDataModel : IDataModel
    {
        public string _id { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get; set; }
        public string TrigersInlevelName { get; set; }
        public string ProductType { get; set; }
        public int DifficultyLevel { get; set; }
        public float InWhatMinutes { get; set; }
        public DateTime TrigerdTime { get; set; }
    }
}
