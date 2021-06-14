using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Ninject;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.UnityManager;
using Appneuron.Models;
using Appneuron.Services;
using System.Threading.Tasks;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.UnityManager
{

    public class AdvEventUnityManager : MonoBehaviour
    {
        private string AdvEventsRequestPath;

        private IAdvEventDal _advEventDal;
        private IRestClientServices _restClientServices;
        private ICryptoServices _cryptoServices;

        private IdUnityManager idUnityManager;
        private DifficultySingletonModel difficultySingletonModel;
        private LocalDataService localDataService;

        private void Awake()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                _advEventDal = kernel.Get<IAdvEventDal>();
                _restClientServices = kernel.Get<IRestClientServices>();
                _cryptoServices = kernel.Get<ICryptoServices>();
            }

        }

        private async void Start()
        {
            idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            AdvEventsRequestPath = WebApiConfigService.ClientWebApiLink + WebApiConfigService.AdvEventsRequestName;
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

            List<string> FolderNameList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService
                                                                                         .SaveTypePath
                                                                                         .AdvEventDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _advEventDal.SelectAsync(ComponentsConfigService.AdvEventDataPath + fileName);
                var result = await _restClientServices.PostAsync<System.Object>(AdvEventsRequestPath, dataModel);
                if (result.Success)
                {
                    await _advEventDal.DeleteAsync(ComponentsConfigService.AdvEventDataPath + fileName);
                }
            }
        }



        public async Task SendAdvEventData(string Tag,
            string levelName,
            float GameSecond)
        {

            int difficultyLevel = difficultySingletonModel.CurrentDifficultyLevel;

            AdvEventDataModel advEventDataModel = new AdvEventDataModel
            {
                ClientId = await idUnityManager.GetPlayerID(),
                ProjectID = ChurnBlockerSingletonConfigService.Instance.GetProjectID(),
                CustomerID = ChurnBlockerSingletonConfigService.Instance.GetCustomerID(),
                TrigersInlevelName = levelName,
                AdvType = Tag,
                DifficultyLevel = difficultyLevel,
                InMinutes = GameSecond,
                TrigerdTime = DateTime.Now
            };



            var result = await _restClientServices.PostAsync<System.Object>(AdvEventsRequestPath, advEventDataModel);

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