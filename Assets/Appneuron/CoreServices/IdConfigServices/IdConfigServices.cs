using Assets.Appneuron.DataAccessBase.Concrete;
using Assets.Appneuron.DataModelBase.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.CoreServices.IdConfigServices
{
    public static class IdConfigServices
    {

        public enum IdPath
        {
            id

        }
        static string dataPath = Application.persistentDataPath;

        public static Dictionary<IdPath, string> GeneralData = new Dictionary<IdPath, string>
        {
            {IdPath.id, dataPath + "/ChurnBlocker/GeneralData/IdData/"},
        };

        public static void CreateFileDirectories()
        {
            foreach (KeyValuePair<IdPath, string> directory in GeneralData)
            {
                Directory.CreateDirectory(directory.Value);
            }
        }

        public static string GetPlayerID()
        {
            string filepath = GeneralData[IdPath.id];
            string fileName = "_id";
            CustomerIdDAL customerIdDAL = new CustomerIdDAL();
            CustomerIdModel customerIdModel = customerIdDAL.Select(filepath + fileName, new CustomerIdModel());
            return customerIdModel._id;
        }

        public static string CustomerIdPath = GeneralData[IdPath.id];


    }
}
