using Appneuron.DifficultyManagerComponent;
using Appneuron.Models;
using Appneuron.Services;
using AppneuronZeroMq;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.DataModel.Concrete;
using Assets.Appneuron.Core.UnityManager;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class DifficultyUnityManager : MonoBehaviour
{
    private int productId;
    private string url;
    private string playerId;

    private IRestClientServices _restClientServices;

    private void Start()
    {
        using (var kernel = new StandardKernel())
        {

            kernel.Load(Assembly.GetExecutingAssembly());
            _restClientServices = kernel.Get<IRestClientServices>();

        }
        IdUnityManager idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();
        playerId = idUnityManager.GetPlayerID();

        productId = AppneuronProductList.ChurnBlocker;
        url = WebApiConfigService.ClientWebApiLink + WebApiConfigService.MlResultRequestName + "?productId=" + productId;

    }

    public async Task AskDifficulty()
    {
        DifficultyModel difficultyModel = new DifficultyModel();
        var result = await _restClientServices.GetAsync<DifficultyModel>(url);

        if (result.Data == null)
        {
            difficultyModel.CenterOfDifficultyLevel = 0;
            difficultyModel.RangeCount = 2;
           
        }
        else
        {
            difficultyModel = result.Data;
        }

        DifficultyManager.MakeConfing();
        DifficultyManager.AskDifficultyLevelFromServer(difficultyModel);



        using (var subscriber = new ZSocket(ZSocketType.SUB))
        {
            string connect_to = TCPSocketConfigService.Connection;
            subscriber.Connect(connect_to);

            subscriber.Subscribe(playerId + "ChurnBlocker_DDA");

            while (true)
            {
                using (var replyFrame = subscriber.ReceiveFrame())
                {
                    string content = replyFrame.ReadString();


                    difficultyModel = JsonConvert.DeserializeObject<DifficultyModel>(content,
                      new JsonSerializerSettings
                      {
                          PreserveReferencesHandling = PreserveReferencesHandling.Objects
                      });

                    DifficultyManager.AskDifficultyLevelFromServer(difficultyModel);
                }
            }

        }
    }

}
