
using Assets.Appneuron.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.DataModel;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityManager;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityWorkflow
{
    public class AdvEventUnityManager : IAdvEventUnityManager
    {
        private readonly IAdvEventDal _advEventDal;
        private readonly IRestClientServices _restClientServices;
        private readonly ICryptoServices _cryptoServices;
        public AdvEventUnityManager(IAdvEventDal advEventDal,
                    IRestClientServices restClientServices,
                    ICryptoServices cryptoServices)
        {
            _advEventDal = advEventDal;
            _restClientServices = restClientServices;
            _cryptoServices = cryptoServices;
        }

        public void CheckAdvFileAndSendData()
        {
            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            List<string> FolderNameList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.AdvEventDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = _advEventDal.Select(ComponentsConfigServices.AdvEventDataPath + fileName);
                string statuseCode = _restClientServices.Post(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    _advEventDal.Delete(ComponentsConfigServices.AdvEventDataPath + fileName);
                }
            }
        }

        public void SendAdvEventData(string Tag,
            string levelName,
            float GameSecond)
        {
            string playerId = IdConfigServices.GetPlayerID();
            string projectId = ChurnBlockerConfigServices.GetProjectID();
            string CustomerId = ChurnBlockerConfigServices.GetCustomerID();
            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            DateTime moment = DateTime.Now;
            int difficultyLevel = ComponentsConfigServices.CurrentDifficultyLevel;

            string filepath = ComponentsConfigServices.AdvEventDataPath;

            AdvEventDataModel dataModel = new AdvEventDataModel {
                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                TrigersInlevelName = levelName,
                AdvType = Tag,
                DifficultyLevel = difficultyLevel,
                InWhatMinutes = GameSecond,
                TrigerdTime = moment
            };




            string statuseCode = _restClientServices.Post(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            _advEventDal.Insert(filepath + fileName, dataModel);
            
        }
    }
}