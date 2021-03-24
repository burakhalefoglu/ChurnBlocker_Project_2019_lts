using System.Collections;
using System.Collections.Generic;
using Assets.Appneuron.CoreServices.RestClientServices.Abstract;
using RestSharp;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Appneuron.CoreServices.RestClientServices.Concrete.RestSharp
{
    public class RestSharpServices : IRestClientServices
    {

        public IRestResponse Get<IRestResponse>(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = (IRestResponse)client.Execute(request);
            return response;

        }

        public string Post(string url, object serializeObject)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            var jsonObject = JsonConvert.SerializeObject(serializeObject);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Debug.Log(response.Content);
            return response.StatusCode.ToString();
        }

        public string Delete(string url)
        {
            throw new System.NotImplementedException();
        }

        public string Put(string url)
        {
            throw new System.NotImplementedException();
        }

    }
}
