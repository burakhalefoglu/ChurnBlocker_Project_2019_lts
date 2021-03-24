
using Assets.Appneuron.DataModelBase.Abstract;
using System;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.GeneralDataComponent.DataModel
{

    [Serializable]
    public class GeneralDataModel : IDataModel
    {

        public string _id { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get; set; }
        public int GameType { get; set; }
        public int PlayersDifficultylevel { get; set; }
        public int GraphStyle { get; set; }

    }
}
