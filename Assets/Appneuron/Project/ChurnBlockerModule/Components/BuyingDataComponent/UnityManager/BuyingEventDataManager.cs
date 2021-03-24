﻿
using Assets.Appneuron.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager
{
    public class BuyingEventDataManager : IBuyingEventDataManager
    {
        private readonly IBuyingEventDal _buyingEventDal;
        private readonly IRestClientServices _restClientServices;
        private readonly ICryptoServices _cryptoServices;
        public BuyingEventDataManager(IBuyingEventDal buyingEventDal,
            IRestClientServices restClientServices,
            ICryptoServices cryptoServices
            )
        {
            _buyingEventDal = buyingEventDal;
            _restClientServices = restClientServices;
            _cryptoServices = cryptoServices;
        }

     

        public void CheckAdvFileAndSendData()
        {


            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.BuyingEventDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = _buyingEventDal.Select(ComponentsConfigServices.BuyingEventDataPath + fileName);
                string statuseCode = _restClientServices.Post(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    _buyingEventDal.Delete(ComponentsConfigServices.AdvEventDataPath + fileName);
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

            BuyingEventDataModel dataModel = new BuyingEventDataModel {

                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                TrigersInlevelName = levelName,
                ProductType = Tag,
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
            _buyingEventDal.Insert(filepath + fileName, dataModel);
        }

    }
}
