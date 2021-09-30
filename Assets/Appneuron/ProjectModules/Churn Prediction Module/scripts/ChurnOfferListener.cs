using AppneuronUnity.ProductModules.ChurnPredictionModule;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers;
using System.Threading.Tasks;
using UnityEngine;

public class ChurnOfferListener : MonoBehaviour
{
    public bool IsGotOffer;

    private void Start()
    {
        IsGotOffer = false;
    }

    public async Task SendOfferData()
    {
        var churnPredictionWorkerModule = GameObject.FindGameObjectWithTag("ChurnPrediction").GetComponent<ChurnPredictionWorkerModule>();
        var churnPredictionModule = GameObject.FindGameObjectWithTag("ChurnPrediction").GetComponent<ChurnPredictionModule>();
        await churnPredictionModule.SendOfferBehaviorData(churnPredictionWorkerModule.GetRemoteOffer().OfferId,
            IsGotOffer);
    }
}
