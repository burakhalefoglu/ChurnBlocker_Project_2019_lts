using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionService : MonoBehaviour
{
    private bool isConnected;
    private float timer;
    public float WaitTimeInSeconds;

    [HideInInspector]
    public bool IsConnected
    {
        get
        {
            return isConnected;

        }
    }


    void Update()
    {

        timer += Time.deltaTime;
        if (timer >= WaitTimeInSeconds)
        {
            timer = 0;
            StartCoroutine(GetRequest("https://google.com"));

        }


    }


    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                isConnected = false;
            }
            else
            {
                isConnected = true;
            }
        }
    }
}
