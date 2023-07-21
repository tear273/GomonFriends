using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Ganet : MonoBehaviour
{
    [SerializeField]
    UILabel ganetNum_Label;

    [SerializeField]
    UILabel price_Label;

    [SerializeField]
    UIButton purchase_btn;
    [SerializeField]
    UITexture purchase_image;

    GanetStoreChart.Item item;

    public void SetData(GanetStoreChart.Item item)
    {
        this.item = item;

        ganetNum_Label.text = item.Num + "개";
        price_Label.text = "₩ " + item.Price;
        Texture image = Resources.Load<Texture>(item.ImagePath);
        purchase_image.mainTexture = image;
    }

    private void Start()
    {
        EventDelegate _event = new EventDelegate(OnClickPurchase);
        purchase_btn.onClick.Add(_event);
    }

    void OnClickPurchase()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        StaticManager.IAP.BuyGanet(item.ProductID, () => {
            int cal = StaticManager.Backend.backendGameData.UserData.Ganet + int.Parse(item.Num);
            StaticManager.Backend.backendGameData.UserData.SetGanet(cal);
            StaticManager.Backend.backendGameData.UserData.Update((callback) => {
                if (callback.IsSuccess())
                {
                    StaticManager.UI.AlertUI.OpenUI("Info", "가넷 " + item.Num + "개 구입 완료했습니다.");
                    GameManager.Instance.Ganet_Label.text = cal.ToString();
                }
            });
        });

    }

}
