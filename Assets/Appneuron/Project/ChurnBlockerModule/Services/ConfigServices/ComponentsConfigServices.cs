﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices
{
    public static class ComponentsConfigServices
    {

        public enum SaveTypePath
        {
            AdvEventDataModel,
            CustomerIdModel,
            SuccessSaveInfo,
            LevelBaseDieDataModel,
            EveryLoginLevelDatasModel,
            LevelsAvarageDatasModel,
            DailySessionDataModel,
            GameSessionEveryLoginDataModel,
            LevelBaseSessionDataModel,
            GeneralDataModel,
            BuyingEventDataModel
        }

        static string dataPath = Application.persistentDataPath;

        public static Dictionary<SaveTypePath, string> ComponentsData = new Dictionary<SaveTypePath, string>
        {
            {SaveTypePath.AdvEventDataModel, dataPath + "/ChurnBlocker/ComponentsData/AdvEventDataModel/"},
            {SaveTypePath.CustomerIdModel, dataPath + "/ChurnBlocker/ComponentsData/CustomerIdModel/"},
            {SaveTypePath.SuccessSaveInfo, dataPath + "/ChurnBlocker/ComponentsData/SuccessSaveInfo/"},
            {SaveTypePath.LevelsAvarageDatasModel, dataPath + "/ChurnBlocker/ComponentsData/LevelsAvarageDatasModel/"},
            {SaveTypePath.LevelBaseDieDataModel, dataPath + "/ChurnBlocker/ComponentsData/LevelBaseDieDataModel/"},
            {SaveTypePath.EveryLoginLevelDatasModel, dataPath + "/ChurnBlocker/ComponentsData/EveryLoginLevelDatasModel/"},
            {SaveTypePath.DailySessionDataModel, dataPath + "/ChurnBlocker/ComponentsData/DailySessionDataModel/"},
            {SaveTypePath.GameSessionEveryLoginDataModel, dataPath + "/ChurnBlocker/ComponentsData/GameSessionEveryLoginDataModel/"},
            {SaveTypePath.LevelBaseSessionDataModel, dataPath + "/ChurnBlocker/ComponentsData/LevelBaseSessionDataModel/"},
            {SaveTypePath.GeneralDataModel, dataPath + "/ChurnBlocker/ComponentsData/GeneralDataModel/"},
            {SaveTypePath.BuyingEventDataModel, dataPath + "/ChurnBlocker/ComponentsData/BuyingEventDataModel/"}
        };

        public static string AdvEventDataPath = ComponentsData[SaveTypePath.AdvEventDataModel];
        public static string CustomerIdPath = ComponentsData[SaveTypePath.CustomerIdModel];
        public static string SuccessSaveInfoPath = ComponentsData[SaveTypePath.SuccessSaveInfo];
        public static string LevelsAvarageDatasPath = ComponentsData[SaveTypePath.LevelsAvarageDatasModel];
        public static string LevelBaseDieDataPath = ComponentsData[SaveTypePath.LevelBaseDieDataModel];
        public static string EveryLoginLevelDatasPath = ComponentsData[SaveTypePath.EveryLoginLevelDatasModel];
        public static string DailySessionDataPath = ComponentsData[SaveTypePath.DailySessionDataModel];
        public static string GameSessionEveryLoginDataPath = ComponentsData[SaveTypePath.GameSessionEveryLoginDataModel];
        public static string LevelBaseSessionDataPath = ComponentsData[SaveTypePath.LevelBaseSessionDataModel];
        public static string GeneralDataPath = ComponentsData[SaveTypePath.GeneralDataModel];
        public static string BuyingEventDataPath = ComponentsData[SaveTypePath.BuyingEventDataModel];
        




        public static void CreateFileVisualDataDirectories()
        {
            foreach (KeyValuePair<SaveTypePath, string> directory in ComponentsData)
            {
                Directory.CreateDirectory(directory.Value);
            }
        }

        public static List<string> GetVisualDataFilesName(SaveTypePath fileType)
        {
            DirectoryInfo dir = new DirectoryInfo(ComponentsData[fileType]);
            FileInfo[] info = dir.GetFiles("*" + ".data");
            List<string> fileNames = new List<string>();
            foreach (FileInfo f in info)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(f.FullName));
            }
            return fileNames;
        }


        public static int GraphStyle = 0;
        public static int GameType = 0;


        public static int CurrentDifficultyLevel = 0;
        public static int CenterOfDifficultyLevel = 0;
        public static int MinOfDifficultyLevelRange = 0;
        public static int MaxOfDifficultyLevelRange = 0;


    }
}