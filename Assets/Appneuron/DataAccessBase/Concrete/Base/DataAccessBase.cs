using Assets.Appneuron.CoreServices.SaveDataServices.Abstract;
using Assets.Appneuron.DataModelBase.Abstract;
using Ninject;
using System.Reflection;

namespace Assets.Appneuron.DataAccessBase.Concrete.Base
{
    public class DataAccessBase<TDataModel>
    where TDataModel : class, IDataModel, new()
    {

        public void Insert(string filePath, TDataModel datamodel)
        {
            using (var kernel = new StandardKernel())
            {
                IDataSaveServices DataAccess;
                kernel.Load(Assembly.GetExecutingAssembly());
                DataAccess = kernel.Get<IDataSaveServices>();
                DataAccess.Insert(filePath, datamodel);

            }
        }

        public void Delete(string filePath)
        {
            using (var kernel = new StandardKernel())
            {
                IDataSaveServices DataAccess;
                kernel.Load(Assembly.GetExecutingAssembly());
                DataAccess = kernel.Get<IDataSaveServices>();
                DataAccess.Delete(filePath);
            }
        }

        public TDataModel Select(string filePath, TDataModel datamodel)
        {
            using (var kernel = new StandardKernel())
            {
                IDataSaveServices DataAccess;
                kernel.Load(Assembly.GetExecutingAssembly());
                DataAccess = kernel.Get<IDataSaveServices>();
                var data = DataAccess.Select(filePath, datamodel);
                return data;

            }
        }



    }

}
