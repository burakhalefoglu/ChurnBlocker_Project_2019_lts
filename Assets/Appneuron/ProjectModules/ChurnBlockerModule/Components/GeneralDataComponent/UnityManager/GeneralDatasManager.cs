
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

        void Start()
        {
            
            idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            StartCoroutine(LateStart(1));
        }

        IEnumerator LateStart(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            if (!checkFileExist())
            {
                AddGeneralDatas();
            }
        }

        void AddGeneralDatas()
        {

            string WebApilink = ChurnBlockerConfigService.GetWebApiLink();

            var result = _restClientServices.Post(WebApilink, new GeneralDataModel
            {
                _id = idUnityManager.GetPlayerID(),
                ProjectID = ChurnBlockerConfigService.GetProjectID(),
                CustomerID = ChurnBlockerConfigService.GetCustomerID(),
                PlayersDifficultylevel = 0,
            });

            if (result.Success)
            {

                SavePlayerSuccessSaveGeneralDataInfo();
                Debug.Log("Başarılı....");
                return;
            }
            Debug.Log(" başarısız oldu : " + result.StatuseCode);

        }


        void SavePlayerSuccessSaveGeneralDataInfo()
        {
            string fileName = "GeneralDataSaved";
            string filepath = ComponentsConfigService.GeneralDataPath + fileName;

            _successSaveInfoDal.Insert(filepath, new SuccessSaveInfo
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