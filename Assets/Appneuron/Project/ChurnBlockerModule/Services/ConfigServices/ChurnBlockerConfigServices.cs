
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices
{
    public class ChurnBlockerConfigServices
    {
        public static string GetWebApiLink()
        {
            JObject appneuronJsonFile = JObject.Parse(File.ReadAllText(Application.streamingAssetsPath + "/ChurnBlocker.json"));
            string[] obj = new string[3];
            int i = 0;
            foreach (JProperty property in appneuronJsonFile.Properties())
            {
                obj[i] = property.Value.ToString();
                i++;

            }

            return obj[0];
        }

        public static string GetProjectID()
        {
            JObject appneuronJsonFile = JObject.Parse(File.ReadAllText(Application.streamingAssetsPath + "/DynamicDifficultyAdjustment.json"));
            string[] obj = new string[3];
            int i = 0;
            foreach (JProperty property in appneuronJsonFile.Properties())
            {
                obj[i] = property.Value.ToString();
                i++;

            }

            return obj[1];

        }

        public static string GetCustomerID()
        {
            JObject appneuronJsonFile = JObject.Parse(File.ReadAllText(Application.streamingAssetsPath + "/DynamicDifficultyAdjustment.json"));
            string[] obj = new string[3];
            int i = 0;
            foreach (JProperty property in appneuronJsonFile.Properties())
            {
                obj[i] = property.Value.ToString();
                i++;

            }

            return obj[2];

        }

       

    }
}


