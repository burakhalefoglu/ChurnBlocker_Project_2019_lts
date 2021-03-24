using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.GeneralDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.GeneralDataComponent.DataModel;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.GeneralDataComponent.UnityWorkflow
{
    public class GeneralDatasManager : MonoBehaviour
    {
        private readonly IRestClientServices _restClientServices;
        private readonly ISuccessSaveInfoDal _successSaveInfoDal;
        private readonly IGeneralDataDal _generalDataDal;
        public GeneralDatasManager(IRestClientServices restClientServices,
            ISuccessSaveInfoDal successSaveInfoDal,
            IGeneralDataDal generalDataDal)
        {
            _restClientServices = restClientServices;
            _successSaveInfoDal = successSaveInfoDal;
            _generalDataDal = generalDataDal;
        }

        void Start()
        {
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
            string playerId = IdConfigServices.GetPlayerID();
            string projectId = ChurnBlockerConfigServices.GetProjectID();
            string CustomerId = ChurnBlockerConfigServices.GetCustomerID();
            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            int GameType = ComponentsConfigServices.GameType;
            int GraphStyle = ComponentsConfigServices.GraphStyle;

            
                string statuseCode = _restClientServices.Post(WebApilink, new GeneralDataModel
                {
                    _id = playerId,
                    ProjectID = projectId,
                    CustomerID = CustomerId,
                    GameType = GameType,
                    PlayersDifficultylevel = 0,
                    GraphStyle = GraphStyle
                });
                if (statuseCode == "Created")
                {

                    SavePlayerSuccessSaveGeneralDataInfo();
                    Debug.Log("Başarılı....");
                    return;
                }
                Debug.Log(" başarısız oldu : " + statuseCode);
            

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