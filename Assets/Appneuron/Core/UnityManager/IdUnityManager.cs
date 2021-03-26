using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Ninject;
using static Assets.Appneuron.Core.CoreServices.IdConfigService.IdConfigServices;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.DataAccessBase;
using Assets.Appneuron.Core.DataModelBase.Concrete;

namespace Assets.Appneuron.Core.UnityManager
{
    public class IdUnityManager : MonoBehaviour
    {
        private IIdDal _bSIdDal;
        private ICryptoServices _cryptoServices;

        private void Awake()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                _bSIdDal = kernel.Get<IIdDal>();
                _cryptoServices = kernel.Get<ICryptoServices>();
            }


            CreateFileDirectories();
            SaveIdOnSaveFolder();
        }
        private void SaveIdOnSaveFolder()
        {
            string filepath = CustomerIdPath;
            string fileName = "_id";

            string savePath = filepath + fileName + ".data";

            if (!File.Exists(savePath))
            {
                string id = GenerateId();
                _bSIdDal.Insert(filepath + fileName, new CustomerIdModel
                {
                    _id = id
                });

            }
        }
        public string GetPlayerID()
        {
            string filepath = GeneralData[IdPath.id];
            string fileName = "_id";
            var customerIdModel = _bSIdDal.Select(filepath + fileName);
            return customerIdModel._id;
        }

        private string GenerateId()
        {
            var id = _cryptoServices.GetRandomHexNumber(32);
            return id;

        }

    }
}
