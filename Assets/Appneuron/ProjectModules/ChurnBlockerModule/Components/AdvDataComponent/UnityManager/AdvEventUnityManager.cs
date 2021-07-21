using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Ninject;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.UnityManager;
using Appneuron.Models;
using Appneuron.Services;
using System.Threading.Tasks;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.UnityManager
{

    public class AdvEventUnityManager : MonoBehaviour
    {
        private IAdvEventDal _advEventDal;
        private ICryptoServices _cryptoServices;
        private IKafkaMessageBroker _kafkaMessageBroker;

        private DifficultySingletonModel difficultySingletonModel;
        private LocalDataService localDataService;

        private void Awake()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                _advEventDal = kernel.Get<IAdvEventDal>();
                _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                _cryptoServices = kernel.Get<ICryptoServices>();
            }

        }

        private async void Start()
        {
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

            List<string> FolderNameList = ComponentsConfigService.GetSavedDataFilesNames(ComponentsConfigService
                                                                                         .SaveTypePath
                                                                                         .AdvEventDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _advEventDal.SelectAsync(ComponentsConfigService.AdvEventDataPath + fileName);
                
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _advEventDal.DeleteAsync(ComponentsConfigService.AdvEventDataPath + fileName);
                }
            }
        }



        public async Task SendAdvEventData(string Tag,
            string levelName,
            float GameSecond,
            string clientId)
        {

            int difficultyLevel = difficultySingletonModel.CurrentDifficultyLevel;

            AdvEventDataModel advEventDataModel = new AdvEventDataModel
            {
                ClientId = clientId,
                ProjectID = ChurnBlockerSingletonConfigService.Instance.GetProjectID(),
                CustomerID = ChurnBlockerSingletonConfigService.Instance.GetCustomerID(),
                TrigersInlevelName = levelName,
                AdvType = Tag,
                DifficultyLevel = difficultyLevel,
                InMinutes = GameSecond,
                TrigerdTime = DateTime.Now
            };


            var result = await _kafkaMessageBroker.SendMessageAsync(advEventDataModel);
            if (result.Success)
            {
                return;
            }

            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigService.AdvEventDataPath + fileName;

            await _advEventDal.InsertAsync(filepath, advEventDataModel);

        }
    }
}