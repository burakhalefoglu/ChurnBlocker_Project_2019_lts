using Appneuron.DifficultyManagerComponent;
using Appneuron.Models;
using Appneuron.Services;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.UnityManager;
using Ninject;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class DifficultyUnityManager : MonoBehaviour
{
    private int productId;
    private string url;

    private IRestClientServices _restClientServices;

    private void Start()
    {
        using (var kernel = new StandardKernel())
        {

            kernel.Load(Assembly.GetExecutingAssembly());
            _restClientServices = kernel.Get<IRestClientServices>();

        }
        IdUnityManager idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();

        productId = AppneuronProductList.ChurnBlocker;
        url = WebApiConfigService.ClientWebApiLink + WebApiConfigService.MlResultRequestName + "?productId=" + productId;

    }

    public async Task AskDifficulty()
    {
        DifficultyModel difficultyModel = new DifficultyModel();
        var result = await _restClientServices.GetAsync<DifficultyModel>(url);

        if (result.Data == null)
        {
            difficultyModel.CenterOfDifficultyLevel = 0;
            difficultyModel.RangeCount = 2;
           
        }
        else
        {
            difficultyModel = result.Data;
        }

        DifficultyManager.MakeConfing();
        DifficultyManager.AskDifficultyLevelFromServer(difficultyModel);
    }

}
