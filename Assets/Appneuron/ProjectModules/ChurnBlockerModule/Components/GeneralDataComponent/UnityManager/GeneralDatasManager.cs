
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.GeneralDataComponent.DataModel;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using Ninject;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.ConfigServices;

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
            StartCoroutine(LateStart(1));
            idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();


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
            string playerId = idUnityManager.GetPlayerID();
            string projectId = ChurnBlockerConfigServices.GetProjectID();
            string CustomerId = ChurnBlockerConfigServices.GetCustomerID();
            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            int GameType = ComponentsConfigServices.GameType;
            int GraphStyle = ComponentsConfigServices.GraphStyle;


            var result = _restClientServices.Post(WebApilink, new GeneralDataModel
            {
                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                GameType = GameType,
                PlayersDifficultylevel = 0,
                GraphStyle = GraphStyle
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
            string filepath = ComponentsConfigServices.GeneralDataPath;
            string fileName = "GeneralDataSaved";

            _successSaveInfoDal.Insert(filepath + fileName, new SuccessSaveInfo
            {
                IsSaved = true
            });

        }

        bool checkFileExist()
        {
            string filepath = ComponentsConfigServices.GeneralDataPath;
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