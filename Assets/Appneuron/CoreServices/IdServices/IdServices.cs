using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.DataAccessBase.Concrete;
using Assets.Appneuron.DataModelBase.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Appneuron.CoreServices.IdServices
{
    public class IdServices : MonoBehaviour
    {
        private void Awake()
        {
            IdConfigServices.IdConfigServices.CreateFileDirectories();
            SaveIdOnSaveFolder();
        }
        void SaveIdOnSaveFolder()
        {
            string filepath = IdConfigServices.IdConfigServices.CustomerIdPath;
            string fileName = "_id";

            string savePath = filepath + fileName + ".data";

            if (!File.Exists(savePath))
            {
                string id = GenerateId();
                CustomerIdDAL customerIdDAL = new CustomerIdDAL();
                customerIdDAL.Insert(filepath + fileName, new CustomerIdModel
                {
                    _id = id
                });

            }
        }

        string GenerateId()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var cr = kernel.Get<ICryptoServices>();
                var id = cr.GetRandomHexNumber(32);
                return id;
            }
        }

    }
}
