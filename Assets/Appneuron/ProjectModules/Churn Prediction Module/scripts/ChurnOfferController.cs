using System.Globalization;
using AppneuronUnity.ProductModules.ChurnPredictionModule;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChurnOfferController : MonoBehaviour
{
    private ChurnPredictionWorkerModule churnPredictionWorkerModule;
    private RemoteOfferEventModel remoteOfferModel;

    void Start()
    {
        churnPredictionWorkerModule = GameObject.FindGameObjectWithTag("ChurnPrediction").GetComponent<ChurnPredictionWorkerModule>();

        var isChurnResult = churnPredictionWorkerModule.GetChurnResult();
        if (isChurnResult)
        {
            CalculateChurnSettings();
        }
        churnPredictionWorkerModule.IsChurnResult += ChurnPredictionWorkerModule_IsChurnResult;

    }

    private async Task ChurnPredictionWorkerModule_IsChurnResult()
    {
        var churnPredictionModule = GameObject.FindGameObjectWithTag("ChurnPrediction").GetComponent<ChurnPredictionModule>();
        var CanOpenOfferPage = await churnPredictionModule.GetOfferResultFromLocal(remoteOfferModel.OfferId);
        if(CanOpenOfferPage)
            CalculateChurnSettings();
    }

    private void CalculateChurnSettings()
    {
        remoteOfferModel = churnPredictionWorkerModule.GetRemoteOffer();
        if (remoteOfferModel == null)
            return;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        var offer = gameObject.transform.GetChild(0).gameObject.GetComponent<Offer>();
        offer.FirstPrice.text = remoteOfferModel.FirstPrice.ToString(CultureInfo.InvariantCulture);
        offer.NewPrice.text = remoteOfferModel.LastPrice.ToString(CultureInfo.InvariantCulture);
        offer.ChangeLastDateTime(remoteOfferModel.StartTime, remoteOfferModel.FinishTime);

        offer.GiftImage.enabled = false;
        if (remoteOfferModel.IsGift)
        {
            offer.GiftImage.enabled = true;

            var tex = new Texture2D(2, 2);
            tex.LoadImage(remoteOfferModel.GiftTexture);

            offer.GiftImage.GetComponent<Image>().sprite =
                Sprite.Create(tex, new Rect(0, 0, 128, 128), new Vector2());
        }

        ItemController ıtemController = GameObject.FindGameObjectWithTag("Items").GetComponent<ItemController>();
        ıtemController.SetItems(remoteOfferModel.ProductModels);

    }


}
