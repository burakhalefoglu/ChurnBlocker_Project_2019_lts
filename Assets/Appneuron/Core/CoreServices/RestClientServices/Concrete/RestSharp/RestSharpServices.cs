using System.Collections;
using System.Collections.Generic;
using RestSharp;
using UnityEngine;
using Newtonsoft.Json;
using System.Net;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.CoreServices.ResultService;
using System.Threading.Tasks;
using Assets.Appneuron.Core.DataModel.Concrete;

namespace Assets.Appneuron.Core.CoreServices.RestClientServices.Concrete.RestSharp
{
    public class RestSharpServices : IRestClientServices
    {

        public async Task<IDataResult<T>> GetAsync<T>(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            var userData = JsonConvert.DeserializeObject<T>(response.Content,
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });

            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            if (numericStatusCode == 200)
            {
                return new SuccessDataResult<T>(userData, statuseCode: numericStatusCode);
            }
            return new ErrorDataResult<T>(userData, statuseCode: numericStatusCode);

        }

        public async Task<IDataResult<T>> PostAsync<T>(string url, object sendObject)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + TokenSingletonModel.Instance.Token);
            var jsonObject = JsonConvert.SerializeObject(sendObject);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);

            IRestResponse response = await client.ExecuteAsync(request);


            var data = JsonConvert.DeserializeObject<T>(response.Content,
                              new JsonSerializerSettings
                              {
                                  PreserveReferencesHandling = PreserveReferencesHandling.Objects
                              });

            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            Debug.Log(response.Content);
            if (numericStatusCode == 201 || numericStatusCode == 200)
            {
                return new SuccessDataResult<T>(data, numericStatusCode);
            }
            return new ErrorDataResult<T>(data, numericStatusCode);
        }

        public IResult Delete(string url)
        {
            throw new System.NotImplementedException();
        }

        public IResult Put(string url)
        {
            throw new System.NotImplementedException();
        }

    }
}
