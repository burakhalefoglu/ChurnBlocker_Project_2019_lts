
using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using Assets.Appneuron.UnityWorkflowBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.BuyingDataComponent.UnityManager
{
    public class BuyingEventDataManager
    {
        public void CheckAdvFileAndSendData()
        {
            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            BaseVisualizationDataManager<BuyingEventDataModel> baseDataWorkflow =
                new BaseVisualizationDataManager<BuyingEventDataModel>();

            BuyingEventDataModel dataModel = new BuyingEventDataModel();
            BuyingEventDAL modelDal = new BuyingEventDAL();

            List<string> FolderList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.BuyingEventDataModel);
            foreach (var item in FolderList)
            {
                dataModel = modelDal.Select(ComponentsConfigServices.BuyingEventDataPath + item, dataModel);
                string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    modelDal.Delete(ComponentsConfigServices.AdvEventDataPath + item);
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

            BuyingEventDataModel dataModel = new BuyingEventDataModel();

            BaseVisualizationDataManager<BuyingEventDataModel> baseDataWorkflow =
            new BaseVisualizationDataManager<BuyingEventDataModel>();

            BuyingEventDAL dataAccess = new BuyingEventDAL();

            string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            baseDataWorkflow.SaveData(filepath, new BuyingEventDataModel
            {
                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                TrigersInlevelName = levelName,
                ProductType = Tag,
                DifficultyLevel = difficultyLevel,
                InWhatMinutes = GameSecond,
                TrigerdTime = moment

            }, dataAccess);
        }

    }
}
