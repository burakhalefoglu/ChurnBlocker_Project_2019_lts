using Assets.Appneuron.CoreServices.IdConfigServices;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.DataAccess;
using Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.DataModel;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;
using Assets.Appneuron.UnityWorkflowBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityWorkflow
{
    public class AdvEventManager
    {

        public void CheckAdvFileAndSendData()
        {
            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            BaseVisualizationDataManager<AdvEventDataModel> baseDataWorkflow =
                new BaseVisualizationDataManager<AdvEventDataModel>();


            AdvEventDataModel dataModel = new AdvEventDataModel();
            AdvEventDAL modelDal = new AdvEventDAL();

            List<string> FolderNameList = ComponentsConfigServices.GetVisualDataFilesName(ComponentsConfigServices.SaveTypePath.AdvEventDataModel);
            foreach (var folderName in FolderNameList)
            {
                dataModel = modelDal.Select(ComponentsConfigServices.AdvEventDataPath + folderName, dataModel);
                string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);
                if (statuseCode == "Created")
                {
                    modelDal.Delete(ComponentsConfigServices.AdvEventDataPath + folderName);
                }
            }
        }

        public void SendAdvEventData(string Tag,
            string levelName,
            float GameSecond
            )
        {
            string playerId = IdConfigServices.GetPlayerID();
            string projectId = ChurnBlockerConfigServices.GetProjectID();
            string CustomerId = ChurnBlockerConfigServices.GetCustomerID();
            string WebApilink = ChurnBlockerConfigServices.GetWebApiLink();

            DateTime moment = DateTime.Now;
            int difficultyLevel = ComponentsConfigServices.CurrentDifficultyLevel;

            string filepath = ComponentsConfigServices.AdvEventDataPath;

            BaseVisualizationDataManager<AdvEventDataModel> baseDataWorkflow =
            new BaseVisualizationDataManager<AdvEventDataModel>();

            AdvEventDataModel dataModel = new AdvEventDataModel();
            AdvEventDAL dataAccess = new AdvEventDAL();

            string statuseCode = baseDataWorkflow.SendData(WebApilink, dataModel);

            if (statuseCode == "Created")
            {
                return;
            }
            baseDataWorkflow.SaveData(filepath, new AdvEventDataModel
            {
                _id = playerId,
                ProjectID = projectId,
                CustomerID = CustomerId,
                TrigersInlevelName = levelName,
                AdvType = Tag,
                DifficultyLevel = difficultyLevel,
                InWhatMinutes = GameSecond,
                TrigerdTime = moment

            }, dataAccess);
        }
    }
}