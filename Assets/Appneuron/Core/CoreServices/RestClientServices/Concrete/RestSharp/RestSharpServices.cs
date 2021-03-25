using System.Collections;
using System.Collections.Generic;
using RestSharp;
using UnityEngine;
using Newtonsoft.Json;
using System.Net;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.CoreServices.ResultService;

namespace Assets.Appneuron.Core.CoreServices.RestClientServices.Concrete.RestSharp
{
    public class RestSharpServices : IRestClientServices
    {

        public IDataResult<T> Get<T>(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            var userData = JsonConvert.DeserializeObject<T>(response.Content,
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });

            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            if (numericStatusCode == 200)
            {
                return new SuccessDataResult<T>(userData, statuseCode:numericStatusCode);
            }
            return new ErrorDataResult<T>(userData, statuseCode:numericStatusCode);

        }

        public IResult Post(string url, object sendObject)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            var jsonObject = JsonConvert.SerializeObject(sendObject);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Debug.Log(response.Content);
            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            if(numericStatusCode == 201)
            {
                return new SuccessResult(numericStatusCode);
            }
            return new ErrorResult(numericStatusCode);
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
