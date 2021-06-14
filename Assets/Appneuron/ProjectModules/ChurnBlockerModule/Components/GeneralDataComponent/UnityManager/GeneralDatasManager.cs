
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.DataModel;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using Ninject;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.UnityManager;
using Appneuron.Services;
using System.Threading.Tasks;
using System;
using Appneuron.Models;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.UnityManager
{
    public class GeneralDatasManager : MonoBehaviour
    {
        private string GeneralDatasRequestPath;

        private IRestClientServices _restClientServices;
        private ISuccessSaveInfoDal _successSaveInfoDal;
        private IGeneralDataDal _generalDataDal;

        private IdUnityManager idUnityManager;
        private LocalDataService localDataService;

        private void Awake()
        {
            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                _successSaveInfoDal = kernel.Get<ISuccessSaveInfoDal>();
                _generalDataDal = kernel.Get<IGeneralDataDal>();
                _restClientServices = kernel.Get<IRestClientServices>();
            }
        }

        public async void Start()
        {

            idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            GeneralDatasRequestPath = WebApiConfigService.ClientWebApiLink + WebApiConfigService.GeneralDatasRequestName;
            await LateStart(3);
        }

        async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            localDataService.CheckLocalData += AddGeneralDatas;

            if (!checkFileExist())
            {
                await AddGeneralDatas();
            }
        }

        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= AddGeneralDatas;

        }

        async Task AddGeneralDatas()
        {


            var result = await _restClientServices.PostAsync<System.Object>(GeneralDatasRequestPath, new GeneralDataModel
            {
                ClientId = await idUnityManager.GetPlayerID(),
                ProjectID = ChurnBlockerSingletonConfigService.Instance.GetProjectID(),
                CustomerID = ChurnBlockerSingletonConfigService.Instance.GetCustomerID(),
                PlayersDifficultylevel = 0,
            });

            if (result.Success)
            {

                await SavePlayerSuccessSaveGeneralDataInfo();
                Debug.Log("Başarılı....");
                return;
            }
            Debug.Log(" başarısız oldu : " + result.StatuseCode);

        }


        async Task SavePlayerSuccessSaveGeneralDataInfo()
        {
            string fileName = ModelNames.GeneralData;
            string filepath = ComponentsConfigService.GeneralDataPath + fileName;

            await _successSaveInfoDal.InsertAsync(filepath, new SuccessSaveInfo
            {
                IsSaved = true
            });

        }

        bool checkFileExist()
        {
            string filepath = ComponentsConfigService.GeneralDataPath;
            string fileName = ModelNames.GeneralData;

            string savePath = filepath + fileName;

            if (File.Exists(savePath))
            {
                return true;
            }
            return false;
        }

    }

}