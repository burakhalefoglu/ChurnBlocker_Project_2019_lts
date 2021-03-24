using Assets.Appneuron.DataModelBase.Abstract;
using System;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.SessionComponent.DataModel
{

    [Serializable]
    public class LevelBaseSessionDataModel : IDataModel
    {
        public string _id { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get ; set ; }
        public string levelName { get; set; }
        public int DifficultyLevel { get; set; }
        public float SessionTimeMinute { get; set; }
        public DateTime SessionStartTime { get; set; }
        public DateTime SessionFinishTime { get; set; }

    }
}
