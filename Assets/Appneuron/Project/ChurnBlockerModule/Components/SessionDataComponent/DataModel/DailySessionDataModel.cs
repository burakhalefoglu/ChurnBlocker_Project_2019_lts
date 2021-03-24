using Assets.Appneuron.DataModelBase.Abstract;
using System;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataModel
{
    [Serializable]
    public class DailySessionDataModel : IDataModel
    {
        public string _id { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get; set; }
        public int SessionFrequency { get; set; }
        public float TotalSessionTime { get; set; }
        public DateTime TodayTime { get; set; }

    }
}
