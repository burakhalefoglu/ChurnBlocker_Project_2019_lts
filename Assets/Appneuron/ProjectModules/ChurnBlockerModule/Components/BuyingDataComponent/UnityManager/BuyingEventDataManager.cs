using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Ninject;
using System.Reflection;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.ConfigServices;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager
{
    public class BuyingEventDataManager : MonoBehaviour
    {

        private IBuyingEventDal _buyingEventDal;
        private IRestClientServices _restClientServices;
        private ICryptoServices _cryptoServices;


        IdUnityManager ıdService;

        private void Awake()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                _buyingEventDal = kernel.Get<IBuyingEventDal>();
                _restClientServices = kernel.Get<IRestClientServices>();
                _cryptoServices = kernel.Get<ICryptoServices>();
            }

        }

        private void Start()
        {
            ıdService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();

        }
        public void CheckAdvFileAndSendData()
        {


            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.BuyingEventDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = _buyingEventDal.Select(ComponentsConfigServices.BuyingEventDataPath + fileName);
                var result = _restClientServices.Post(WebApilink, dataModel);
                if (result.Success)
                {
                    _buyingEventDal.Delete(ComponentsConfigServices.AdvEventDataPath + fileName);
                }
            }
        }

        public void SendAdvEventData(string Tag,
            string levelName,
            float GameSecond)

        {
            string playerId = ıdService.GetPlayerID();
            string projectId = ChurnBlockerConfigServices.GetProjectID();
            string customerId = ChurnBlockerConfigServices.GetCustomerID();
            string webApilink = ChurnBlockerConfigServices.GetWebApiLink();
            DateTime moment = DateTime.Now;
            int difficultyLevel = ComponentsConfigServices.CurrentDifficultyLevel;
            string filepath = ComponentsConfigServices.AdvEventDataPath;

            BuyingEventDataModel dataModel = new BuyingEventDataModel
            {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                TrigersInlevelName = levelName,
                ProductType = Tag,
                DifficultyLevel = difficultyLevel,
                InWhatMinutes = GameSecond,
                TrigerdTime = moment

            };


            var result = _restClientServices.Post(webApilink, dataModel);

            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            _buyingEventDal.Insert(filepath + fileName, dataModel);
        }

    }
}
