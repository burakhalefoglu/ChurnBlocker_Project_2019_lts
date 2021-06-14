using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataModel
{
    [Serializable]
    public class DailySessionDataModel : IEntity
    {
        public string ClientId { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get; set; }
        public int SessionFrequency { get; set; }
        public float TotalSessionTime { get; set; }
        public DateTime TodayTime { get; set; }

    }
}
