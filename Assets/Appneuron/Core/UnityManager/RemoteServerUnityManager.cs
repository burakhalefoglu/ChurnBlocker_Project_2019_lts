using Appneuron.DifficultyManagerComponent;
using Appneuron.Models;
using Appneuron.Services;
using AppneuronZeroMq;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.Core.UnityManager
{
    public class RemoteServerUnityManager : MonoBehaviour
    {
        private string playerId;

        private void Start()
        {
            IdUnityManager idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
            playerId = idUnityManager.GetPlayerID();

            SubscribeRemoteEvent(playerId);
        }

        public void SubscribeRemoteEvent(string userID)
        {
            // TODO: "socket yapısı genel Appneuron componentine çekilecek. Ürünlere göre aksiyon aldırılacak. ";


            //using (var subscriber = new ZSocket(ZSocketType.SUB))
            //{
            //    string connect_to = TCPSocketConfigService.Connection;
            //    subscriber.Connect(connect_to);

            //    subscriber.Subscribe(userID);

            //    while (true)
            //    {
            //        using (var replyFrame = subscriber.ReceiveFrame())
            //        {
            //            string content = replyFrame.ReadString();


            //            difficultyModel = JsonConvert.DeserializeObject<DifficultyModel>(content,
            //              new JsonSerializerSettings
            //              {
            //                  PreserveReferencesHandling = PreserveReferencesHandling.Objects
            //              });

            //            DifficultyManager.AskDifficultyLevelFromServer(difficultyModel);
            //        }
            //    }

            //}
        }
    }
}
