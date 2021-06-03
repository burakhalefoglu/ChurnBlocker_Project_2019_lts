using Assets.Appneuron.Core.CoreServices.ResultService;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract
{
    public interface IRestClientServices
    {
        Task<IDataResult<T>> GetAsync<T>(string url);
        Task<IDataResult<T>> PostAsync<T>(string url, object sendObject);
    }
}
