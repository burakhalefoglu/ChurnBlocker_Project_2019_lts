﻿using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Assets.Appneuron.CoreServices.SaveDataServices.Abstract;
using Assets.Appneuron.DataModelBase.Abstract;

namespace Assets.Appneuron.CoreServices.SaveDataServices.Concrete.BinaryData
{
    public class BinaryDataSaveServices : IDataSaveServices
    {
        public T Select<T>(string filePath, T model)
        {
            string savePath = filePath + ".data";
            Debug.Log(savePath);
            if (File.Exists(savePath))
            {

                var binaryFormatter = new BinaryFormatter();
                using (var fileStream = File.Open(savePath, FileMode.Open))
                {
                    T dataModel = (T)binaryFormatter.Deserialize(fileStream);
                    return dataModel;
                }

            }
            else
            {
                Debug.LogWarning("Save file doesn't exist.");
            }
            return model;


        }

        public void Insert(string filePath, IDataModel dataModel)
        {

            var binaryFormatter = new BinaryFormatter();


            string savePath = filePath + ".data";
            using (var fileStream = File.Create(savePath))
            {
                binaryFormatter.Serialize(fileStream, dataModel);
            }

        }

        public void Delete(string filePath)
        {
            string saveFilePath = filePath + ".data";
            File.Delete(saveFilePath);
            Debug.Log("File Deleted");

        }

    }
}