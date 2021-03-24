using Assets.Appneuron.DataModelBase.Abstract;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Appneuron.CoreServices.SaveDataServices.Abstract
{
    public interface IDataSaveServices
    {

        void Insert(string filePath, IDataModel dataModel);
        T Select<T>(string filePath, T model);
        void Delete(string filePath);

    }
}
