using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public void SetItems(ProductModelDto[] productModelDtos)
    {
        var childObj = this.gameObject.transform.GetChild(0).gameObject;
        for (int i = 0; i < productModelDtos.Length; i++)
        {
            var spawndChildObj = Instantiate(childObj, new Vector2(0, 0), Quaternion.identity);
            spawndChildObj.transform.SetParent(this.transform);
            Item ıtem = spawndChildObj.gameObject.GetComponent<Item>();
            ıtem.ItemName.text = productModelDtos[i].Name.ToString();
            ıtem.Count.text = productModelDtos[i].Count.ToString();
            ıtem.Image.GetComponent<Image>().sprite =
            Sprite.Create(productModelDtos[i].Image, new Rect(0, 0, 128, 128), new Vector2());
        }
    }


}
