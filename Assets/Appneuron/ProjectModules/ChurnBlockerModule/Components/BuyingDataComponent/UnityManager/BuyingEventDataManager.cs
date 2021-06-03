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
using Appneuron;
using Appneuron.Services;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager
{
    public class BuyingEventDataManager : MonoBehaviour
    {

        private IBuyingEventDal _buyingEventDal;
        private IRestClientServices _restClientServices;
        private ICryptoServices _cryptoServices;


        private IdUnityManager idUnityManager;
        private DifficultySingletonModel difficultySingletonModel;

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
            idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            difficultySingletonModel = DifficultySingletonModel.Instance;

        }

        public async Task CheckAdvFileAndSendData()
        {


            string WebApilink = ChurnBlockerConfigService.GetWebApiLink();

            List<string> FolderList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService
                                                                                      .SaveTypePath
                                                                                      .BuyingEventDataModel);

            foreach (var fileName in FolderList)
            {
                var dataModel = await _buyingEventDal.SelectAsync(ComponentsConfigService.BuyingEventDataPath + fileName);
                var result = await _restClientServices.PostAsync<System.Object>(WebApilink, dataModel);
                if (result.Success)
                {
                    await _buyingEventDal.DeleteAsync(ComponentsConfigService.AdvEventDataPath + fileName);
                }
            }
        }

        public async Task SendAdvEventData(string Tag,
            string levelName,
            float GameSecond)

        {


            int difficultyLevel = difficultySingletonModel.CurrentDifficultyLevel;

            BuyingEventDataModel dataModel = new BuyingEventDataModel
            {

                _id = await idUnityManager.GetPlayerID(),
                ProjectID = ChurnBlockerConfigService.GetProjectID(),
                CustomerID = ChurnBlockerConfigService.GetCustomerID(),
                TrigersInlevelName = levelName,
                ProductType = Tag,
                DifficultyLevel = difficultyLevel,
                InWhatMinutes = GameSecond,
                TrigerdTime = DateTime.Now

            };


            string webApilink = ChurnBlockerConfigService.GetWebApiLink();
            var result = await _restClientServices.PostAsync<System.Object>(webApilink, dataModel);

            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigService.AdvEventDataPath + fileName;

            await _buyingEventDal.InsertAsync(filepath, dataModel);
        }

    }
}
