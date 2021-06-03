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
using Appneuron;
using Appneuron.Services;
using System.Threading.Tasks;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.UnityManager
{

    public class AdvEventUnityManager : MonoBehaviour
    {

        private IAdvEventDal _advEventDal;
        private IRestClientServices _restClientServices;
        private ICryptoServices _cryptoServices;

        private IdUnityManager idUnityManager;
        private DifficultySingletonModel difficultySingletonModel;

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

        private void Start()
        {
            idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            difficultySingletonModel = DifficultySingletonModel.Instance;
        }



        public async Task CheckAdvFileAndSendData()
        {
            string WebApilink = ChurnBlockerConfigService.GetWebApiLink();

            List<string> FolderNameList = ComponentsConfigService.GetVisualDataFilesName(ComponentsConfigService
                                                                                         .SaveTypePath
                                                                                         .AdvEventDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _advEventDal.SelectAsync(ComponentsConfigService.AdvEventDataPath + fileName);
                var result = await _restClientServices.PostAsync<System.Object>(WebApilink, dataModel);
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
                _id = await idUnityManager.GetPlayerID(),
                ProjectID = ChurnBlockerConfigService.GetProjectID(),
                CustomerID = ChurnBlockerConfigService.GetCustomerID(),
                TrigersInlevelName = levelName,
                AdvType = Tag,
                DifficultyLevel = difficultyLevel,
                InWhatMinutes = GameSecond,
                TrigerdTime = DateTime.Now
            };



            string webApilink = ChurnBlockerConfigService.GetWebApiLink();
            var result = await _restClientServices.PostAsync<System.Object>(webApilink, advEventDataModel);

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