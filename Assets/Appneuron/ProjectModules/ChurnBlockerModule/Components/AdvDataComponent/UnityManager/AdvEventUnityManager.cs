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
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.ConfigServices;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.AdvDataComponent.UnityManager
{

    public class AdvEventUnityManager : MonoBehaviour
    {

        private IAdvEventDal _advEventDal;
        private IRestClientServices _restClientServices;
        private ICryptoServices _cryptoServices;

        private IdUnityManager idUnityManager;

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

        }
        public void CheckAdvFileAndSendData()
        {
            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            List<string> FolderNameList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.AdvEventDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = _advEventDal.Select(ComponentsConfigServices.AdvEventDataPath + fileName);
                var result = _restClientServices.Post(WebApilink, dataModel);
                if (result.Success)
                {
                    _advEventDal.Delete(ComponentsConfigServices.AdvEventDataPath + fileName);
                }
            }
        }

        public void SendAdvEventData(string Tag,
            string levelName,
            float GameSecond)
        {
            string playerId = idUnityManager.GetPlayerID();
            string projectId = ChurnBlockerConfigServices.GetProjectID();
            string customerId = ChurnBlockerConfigServices.GetCustomerID();
            string webApilink = ChurnBlockerConfigServices.GetWebApiLink();

            DateTime moment = DateTime.Now;
            int difficultyLevel = ComponentsConfigServices.CurrentDifficultyLevel;

            string filepath = ComponentsConfigServices.AdvEventDataPath;

            AdvEventDataModel dataModel = new AdvEventDataModel
            {
                _id = playerId,
                ProjectID = projectId,
                CustomerID = customerId,
                TrigersInlevelName = levelName,
                AdvType = Tag,
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
            _advEventDal.Insert(filepath + fileName, dataModel);

        }
    }
}