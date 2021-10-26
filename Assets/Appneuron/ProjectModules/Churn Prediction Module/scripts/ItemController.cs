using System.Globalization;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public void SetItems(ProductModel[] productModels)
    {
        var childObj = this.gameObject.transform.GetChild(0).gameObject;
        for (int i = 0; i < productModels.Length; i++)
        {
            var spawnChildObj = Instantiate(childObj, new Vector2(0, 0), Quaternion.identity);
            spawnChildObj.transform.SetParent(this.transform);
            var ıtem = spawnChildObj.gameObject.GetComponent<Item>();
            ıtem.ItemName.text = productModels[i].Name;
            ıtem.Count.text = productModels[i].Count.ToString(CultureInfo.InvariantCulture);

            var tex = new Texture2D(2, 2);
            tex.LoadImage(productModels[i].Image);

            ıtem.Image.GetComponent<Image>().sprite =
            Sprite.Create(tex, new Rect(0, 0, 128, 128), new Vector2());
        }
    }


}
