using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Ninject;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionService : MonoBehaviour
{
    private bool isConnected;
    private float timer;
    public float WaitTimeInSeconds;

    private IRestClientServices _restClientServices;


    private void Start()
    {
        using (var kernel = new StandardKernel())
        {

            kernel.Load(Assembly.GetExecutingAssembly());
            _restClientServices = kernel.Get<IRestClientServices>();

        }
    }


    [HideInInspector]
    public bool IsConnected
    {
        get
        {
            return isConnected;
        }
    }


    async void Update()
    {

        timer += Time.deltaTime;
        if (timer >= WaitTimeInSeconds)
        {
            timer = 0;
            var result =  await _restClientServices.IsInternetConnectedAsync();
            isConnected = result.Success;

        }

    }
}
