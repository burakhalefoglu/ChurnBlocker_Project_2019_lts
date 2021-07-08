using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Ninject;
using System.Reflection;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.UnityManager;
using Appneuron.Models;
using Appneuron.Services;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager
{
    public class BuyingEventDataManager : MonoBehaviour
    {
        private IBuyingEventDal _buyingEventDal;
        private ICryptoServices _cryptoServices;
        private IKafkaMessageBroker _kafkaMessageBroker;


        private IdUnityManager idUnityManager;
        private DifficultySingletonModel difficultySingletonModel;
        private LocalDataService localDataService;

        private void Awake()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                _buyingEventDal = kernel.Get<IBuyingEventDal>();
                _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                _cryptoServices = kernel.Get<ICryptoServices>();
            }

        }

        private async void Start()
        {
            idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();

            difficultySingletonModel = DifficultySingletonModel.Instance;
            await LateStart(3);



        }

        async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await CheckAdvFileAndSendData();

            localDataService.CheckLocalData += CheckAdvFileAndSendData;
        }

        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= CheckAdvFileAndSendData;

        }


        public async Task CheckAdvFileAndSendData()
        {



            List<string> FolderList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService
                                                                                      .SaveTypePath
                                                                                      .BuyingEventDataModel);

            foreach (var fileName in FolderList)
            {
                var dataModel = await _buyingEventDal.SelectAsync(ComponentsConfigService.BuyingEventDataPath + fileName);
                
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
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

                ClientId = await idUnityManager.GetPlayerID(),
                ProjectID = ChurnBlockerSingletonConfigService.Instance.GetProjectID(),
                CustomerID = ChurnBlockerSingletonConfigService.Instance.GetCustomerID(),
                TrigersInlevelName = levelName,
                ProductType = Tag,
                DifficultyLevel = difficultyLevel,
                InWhatMinutes = GameSecond,
                TrigerdTime = DateTime.Now

            };


            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
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
