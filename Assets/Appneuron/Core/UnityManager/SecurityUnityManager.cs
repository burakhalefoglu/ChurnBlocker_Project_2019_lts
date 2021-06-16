using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.DataModel.Concrete;
using Assets.Appneuron.Core.DataAccess;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Appneuron.Services;
using Appneuron.Models;

namespace Assets.Appneuron.Core.UnityManager
{
    public class SecurityUnityManager : MonoBehaviour
    {

        private string filePath;
        private string fileName;
        private string RequestPath;


        private IJwtDal _jwtDal;
        private IRestClientServices _restClientServices;


        private async void Awake()
        {
            filePath = ComponentsConfigService.TokenDataModel;
            fileName = ModelNames.TokenName;
            RequestPath = WebApiConfigService.AuthWebApiLink + WebApiConfigService.ClientTokenRequestName;

            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                _jwtDal = kernel.Get<IJwtDal>();
                _restClientServices = kernel.Get<IRestClientServices>();

            }

            await login();
        }

        private async Task login()
        {
            var tokenmodel = await checkTokenOnfile();
            Debug.Log(tokenmodel.Token);
            if (tokenmodel.Token != "" && tokenmodel.Expiration > DateTime.Now)
            {
                TokenSingletonModel.Instance.Token = tokenmodel.Token;
                return;

            }

            string projectId = ChurnBlockerSingletonConfigService.Instance.GetProjectID();
            string customerId = ChurnBlockerSingletonConfigService.Instance.GetCustomerID();

            JwtRequestModel JwtRequestModel = new JwtRequestModel
            {
                ClientId = SystemInfo.deviceUniqueIdentifier,
                DashboardKey = customerId,
                ProjectId = projectId
            };

            var result = await _restClientServices.PostAsync<JwtResponseModel>
                (RequestPath,
                JwtRequestModel);
            if (result.Success)
            {
                await SaveTokenOnfile(result.Data.Data);
            }
        }

        private async Task<TokenDataModel> checkTokenOnfile()
        {


            var dataModel = await _jwtDal.SelectAsync(filePath + fileName);
            return dataModel;

        }

        private async Task SaveTokenOnfile(TokenDataModel tokenDataModel)
        {
            TokenSingletonModel.Instance.Token = tokenDataModel.Token;
            await _jwtDal.InsertAsync(filePath + fileName, tokenDataModel);
        }

    }

}
