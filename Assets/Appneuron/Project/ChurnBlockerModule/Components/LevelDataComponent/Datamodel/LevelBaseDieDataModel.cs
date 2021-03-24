using Assets.Appneuron.DataModelBase.Abstract;
using System;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.LevelDataComponent.Datamodel
{

    [Serializable]
    public class LevelBaseDieDataModel : IDataModel
    {
        public string _id { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get; set; }
        public int DiyingTimeAfterLevelStarting { get; set; }
        public string levelName { get; set; }
        public int DiyingDifficultyLevel{ get; set; }
        public float DiyingLocationX { get; set; }
        public float DiyingLocationY { get; set; }
        public float DiyingLocationZ { get; set; }
    }
}
