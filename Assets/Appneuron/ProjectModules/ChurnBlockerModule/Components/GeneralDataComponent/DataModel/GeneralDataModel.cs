
using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.DataModel
{

    [Serializable]
    public class GeneralDataModel : IEntity
    {

        public string _id { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get; set; }
        public int GameType { get; set; }
        public int PlayersDifficultylevel { get; set; }
        public int GraphStyle { get; set; }

    }
}
