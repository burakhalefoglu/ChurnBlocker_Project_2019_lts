using System.Collections;
using System.Collections.Generic;

namespace Assets.Appneuron.Core.CoreServices.DataStorageService.Abstract
{
    public interface IRepositoryService<T>
    {

        void Insert(string filePath, T dataModel);
        T Select(string filePath);
        void Delete(string filePath);

    }
}
