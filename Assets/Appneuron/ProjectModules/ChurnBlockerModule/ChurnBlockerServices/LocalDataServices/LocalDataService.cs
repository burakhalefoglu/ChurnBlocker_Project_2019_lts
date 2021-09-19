using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class LocalDataService : MonoBehaviour
{


    ConnectionService connectionService;
    private float timer;
    private float waitSeconds;
    public float WaitMinutes;

    public delegate Task OnCheckLoacalData();
    public event OnCheckLoacalData CheckLocalData;

    void Start()
    {
        connectionService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<ConnectionService>();
        waitSeconds = 60 * WaitMinutes;

    }

    void Update()
    {

        timer += Time.deltaTime;
        if (timer >= waitSeconds)
        {
            timer = 0;
            if (connectionService.IsConnected)
            {
                CheckLocalData();
            }
        }
    }
}
