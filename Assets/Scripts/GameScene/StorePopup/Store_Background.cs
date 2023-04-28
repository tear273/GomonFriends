using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Background : MonoBehaviour
{
    [SerializeField]
    GameObject exit;

    [SerializeField]
    GameObject purchase;

    [SerializeField]
    UILabel exit_Name_Label;

    [SerializeField]
    UILabel purchase_Name_Label;
    [SerializeField]
    UILabel purchase_Price_Label;
    [SerializeField]
    UITexture priceType_Texture;

    [SerializeField]
    UIButton purchase_btn;

    [SerializeField]
    Texture frendship;
    [SerializeField]
    Texture ganet;

    DecoChart.Item item;
    public void SetData(DecoChart.Item item)
    {
        exit_Name_Label.text = item.Name;
        purchase_Name_Label.text = item.Name;
        purchase_Price_Label.text = "X " + item.Price;
        priceType_Texture.mainTexture = item.From == 1 ? frendship : ganet;
        this.item = item;
        state = StaticManager.Backend.backendGameData.DecoData.Deco.ContainsKey(item.Code) ? ContensState.on : ContensState.unPurchase;
    }

    public ContensState state
    {
        set
        {
            switch (value)
            {
                case ContensState.unPurchase:
                    exit.SetActive(false);
                    purchase.SetActive(true);

                    EventDelegate _event = new EventDelegate(OnClickPurchase_Btn);
                    purchase_btn.onClick.Add(_event);

                    break;
                case ContensState.on:
                    exit.SetActive(true);
                    purchase.SetActive(false);
                    break;
            }
        }
    }

    void OnClickPurchase_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        PurchasePopup_Info info = new PurchasePopup_Info();
        info.info = item.Name + "을\n해당 가격에 구매 하시겠습니까?";
        info.price =  item.Price.ToString();
        info.moneyType = item.From;
        //info.thumbnail = Image;
        info.func = () => {
            int price = item.Price;
            if (StaticManager.Backend.backendGameData.UserData.Ganet >= price)
            {
                var cal = StaticManager.Backend.backendGameData.UserData.Ganet -= price;
                StaticManager.Backend.backendGameData.UserData.SetGanet(cal);
                StaticManager.Backend.backendGameData.UserData.Update((callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        StaticManager.Backend.backendGameData.DecoData.SetDeco(item.Code);
                        StaticManager.Backend.backendGameData.DecoData.Update((callback) =>
                        {
                            if (callback.IsSuccess())
                            {
                                GameManager.Instance.Ganet_Label.text = StaticManager.Backend.backendGameData.UserData.Ganet.ToString();
                                state = ContensState.on;
                                StaticManager.UI.AlertUI.OpenUI("Info", item.Name + " 구매 완료!");
                                GameManager.Instance.FriendsPurchase_Popup.gameObject.SetActive(false);
                            }
                            else
                            {
                                StaticManager.Backend.backendGameData.DecoData.Deco.Remove(item.Code);
                            }
                        });
                    }
                    else
                    {
                        StaticManager.Backend.backendGameData.UserData.SetGanet(cal + price);
                    }

                });

            }
            else
            {
                StaticManager.UI.AlertUI.OpenUI("Info", "우정별 갯수가 부족합니다.");
            }
        };

        GameManager.Instance.ShowPurchasePopup(info);
    }
}
