using Appneuron.Core.CoreServices.WebSocketService;
using Appneuron.DifficultyManagerComponent;
using Appneuron.Models;
using Appneuron.Services;
using AppneuronUnity.Core.UnityManager;
using AppneuronZeroMq;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Appneuron.Core.UnityManager
{
    public class RemoteServerUnityController : MonoBehaviour
    {
        private async void Start()
        {
            await MlResultClient.ListenServerManager();
        }


    }
}
