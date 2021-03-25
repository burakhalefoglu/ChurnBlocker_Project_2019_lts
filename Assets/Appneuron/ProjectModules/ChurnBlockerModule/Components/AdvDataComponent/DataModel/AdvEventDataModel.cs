
using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataModel
{
    [Serializable]
    public class AdvEventDataModel : IEntity
    {
        public string _id { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get; set; }
        public string TrigersInlevelName { get; set; }
        public string AdvType { get; set; }
        public int DifficultyLevel { get; set; }
        public float InWhatMinutes { get; set; }
        public DateTime TrigerdTime { get; set; }
    }
}
