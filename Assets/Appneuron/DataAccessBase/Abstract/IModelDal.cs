using Assets.Appneuron.DataModelBase.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.DataAccessBase.Abstract
{
    public interface IModelDal<TDataModel>
         where TDataModel : class, IDataModel, new()
    {
        void Insert(string filePath, TDataModel datamodel);
        void Delete(string filePath);
        TDataModel Select(string filePath, TDataModel datamodel);

    }
}
