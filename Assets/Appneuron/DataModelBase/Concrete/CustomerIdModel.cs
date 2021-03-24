using Assets.Appneuron.DataModelBase.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.DataModelBase.Concrete
{

    [Serializable]
    public class CustomerIdModel : IDataModel
    {
        public string _id { get; set; }
    }

}