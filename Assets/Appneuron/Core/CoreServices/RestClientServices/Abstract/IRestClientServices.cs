using Assets.Appneuron.Core.CoreServices.ResultService;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract
{
    public interface IRestClientServices
    {
        IDataResult<T> Get<T>(string url);
        IResult Post(string url, object sendObject);
        IResult Delete(string url);
        IResult Put(string url);
    }
}
