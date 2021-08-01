using Appneuron.Core.CoreServices.WebSocketService;
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

        private void Awake()
        {
            WebsocketClient.ListenServerManager();
        }


    }
}
