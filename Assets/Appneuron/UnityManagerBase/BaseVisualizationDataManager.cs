
using Assets.Appneuron.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.DataAccessBase.Abstract;
using Assets.Appneuron.DataModelBase.Abstract;
using Ninject;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Appneuron.UnityWorkflowBase
{
    public class BaseVisualizationDataManager<T> where T : class, IDataModel, new()
    {

        public string SendData(string WebApilink, IDataModel dataModel)
        {
            using (var kernel = new StandardKernel())
            {
                IRestClientServices http;
                kernel.Load(Assembly.GetExecutingAssembly());
                http = kernel.Get<IRestClientServices>();
                string statuseCode = http.Post(WebApilink, dataModel);
                return statuseCode;
            }
        }

        public T GetData
            (string filepath,
            string fileName,
            T dataModel,
            IModelDal<T> modelDal)
        {
            return modelDal.Select(filepath + fileName, dataModel);
        }

        public void DeleteData
            (string filepath,
            string fileName,
            IModelDal<T> modelDal)
        {
            modelDal.Delete(filepath + fileName);
        }

        public void SaveData
            (string filepath,
            T dataModel,
            IModelDal<T> modelDal)
        {
            using (var kernel = new StandardKernel())
            {
                ICryptoServices crypto;
                kernel.Load(Assembly.GetExecutingAssembly());
                crypto = kernel.Get<ICryptoServices>();
                string fileName = crypto.GenerateStringName(6);
                modelDal.Insert(filepath + fileName, dataModel);
            }
        }
    }
}

