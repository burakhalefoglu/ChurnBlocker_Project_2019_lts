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
using Appneuron.Models;

namespace Assets.Appneuron.Core.UnityManager
{
    public class IdUnityManager : MonoBehaviour
    {
        private IIdDal _bSIdDal;
        private ICryptoServices _cryptoServices;

        private async void Awake()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                _bSIdDal = kernel.Get<IIdDal>();
                _cryptoServices = kernel.Get<ICryptoServices>();
            }


            CreateFileDirectories();
            await SaveIdOnSaveFolder();
        }
        private async Task SaveIdOnSaveFolder()
        {
            string filepath = CustomerIdPath;
            string fileName = ModelNames.IdName;

            string savePath = filepath + fileName;

            if (!File.Exists(savePath))
            {
                string id = GenerateId();
                await _bSIdDal.InsertAsync(filepath + fileName, new CustomerIdModel
                {
                    _id = id
                });

            }
        }
        public string GetPlayerID()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        public string GenerateId()
        {
            return _cryptoServices.GetRandomHexNumber(32);
        }

    }
}
