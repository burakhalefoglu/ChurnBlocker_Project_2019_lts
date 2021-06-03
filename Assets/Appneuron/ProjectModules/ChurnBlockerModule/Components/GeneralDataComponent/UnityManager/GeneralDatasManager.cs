
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

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.UnityManager
{
    public class GeneralDatasManager : MonoBehaviour
    {

        private IRestClientServices _restClientServices;
        private ISuccessSaveInfoDal _successSaveInfoDal;
        private IGeneralDataDal _generalDataDal;

        private IdUnityManager idUnityManager;

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

            await LateStart(1);
        }

        async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            if (!checkFileExist())
            {
                await AddGeneralDatas();
            }
        }

        async Task AddGeneralDatas()
        {

            string WebApilink = ChurnBlockerConfigService.GetWebApiLink();

            var result = await _restClientServices.PostAsync<System.Object>(WebApilink, new GeneralDataModel
            {
                _id = await idUnityManager.GetPlayerID(),
                ProjectID = ChurnBlockerConfigService.GetProjectID(),
                CustomerID = ChurnBlockerConfigService.GetCustomerID(),
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
            string fileName = "GeneralDataSaved";
            string filepath = ComponentsConfigService.GeneralDataPath + fileName;

            await _successSaveInfoDal.InsertAsync(filepath, new SuccessSaveInfo
            {
                IsSaved = true
            });

        }

        bool checkFileExist()
        {
            string filepath = ComponentsConfigService.GeneralDataPath;
            string fileName = "GeneralDataSaved";

            string savePath = filepath + fileName + ".data";

            if (File.Exists(savePath))
            {
                return true;
            }
            return false;
        }

    }

}