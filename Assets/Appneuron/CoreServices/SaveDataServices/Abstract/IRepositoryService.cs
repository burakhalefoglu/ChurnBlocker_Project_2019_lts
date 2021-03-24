using Assets.Appneuron.DataModelBase.Abstract;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Appneuron.CoreServices.SaveDataServices.Abstract
{
    public interface IRepositoryService<T>
    {

        void Insert(string filePath, T dataModel);
        T Select(string filePath);
        void Delete(string filePath);

    }
}
