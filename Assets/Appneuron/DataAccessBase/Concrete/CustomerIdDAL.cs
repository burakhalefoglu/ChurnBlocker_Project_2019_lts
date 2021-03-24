using Assets.Appneuron.DataAccessBase.Abstract;
using Assets.Appneuron.DataAccessBase.Concrete.Base;
using Assets.Appneuron.DataModelBase.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.DataAccessBase.Concrete
{
    public class CustomerIdDAL : DataAccessBase<CustomerIdModel>, IModelDal<CustomerIdModel>
    {
    }
}
