using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Appneuron.CoreServices.RestClientServices.Abstract
{
    public interface IRestClientServices
    {
        T Get<T>(string url);
        string Post(string url, object SendObject);
        string Delete(string url);
        string Put(string url);
    }
}
