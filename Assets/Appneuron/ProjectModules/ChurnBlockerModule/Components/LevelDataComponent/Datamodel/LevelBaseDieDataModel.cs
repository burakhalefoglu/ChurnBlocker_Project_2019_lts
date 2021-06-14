
using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.Datamodel
{

    [Serializable]
    public class LevelBaseDieDataModel : IEntity
    {
        public string ClientId { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get; set; }
        public int DiyingTimeAfterLevelStarting { get; set; }
        public string levelName { get; set; }
        public int DiyingDifficultyLevel { get; set; }
        public float DiyingLocationX { get; set; }
        public float DiyingLocationY { get; set; }
        public float DiyingLocationZ { get; set; }
    }
}
