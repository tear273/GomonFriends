using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Skin : MonoBehaviour
{
    [SerializeField]
    GameObject exit;

    [SerializeField]
    GameObject purchase;

    [SerializeField]
    UILabel exit_Name_Label;
    [SerializeField]
    UITexture exist_Image;

    [SerializeField]
    UILabel purchase_Name_Label;
    [SerializeField]
    UILabel purchase_Price_Label;
    [SerializeField]
    UITexture purchase_Image;

    [SerializeField]
    UIButton purchase_btn;


    SkinChart.Item item;

    public void SetData(SkinChart.Item item)
    {
        this.item = item;

        exit_Name_Label.text = item.Name;
        purchase_Name_Label.text = item.Name;
        purchase_Price_Label.text = item.Price;
        if (StaticManager.Backend.backendGameData.FriendsData.Skin.ContainsKey(item.OriginCode))
        {
            state = StaticManager.Backend.backendGameData.FriendsData.Skin[item.OriginCode].Contains(item.Code) ? ContensState.on : ContensState.unPurchase;
        }
        else {
            state = ContensState.unPurchase;

        }

        Texture image = Resources.Load<Texture>(item.ResourcePath);
        exist_Image.mainTexture = image;
        purchase_Image.mainTexture = image;
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

    public void OnClickPurchase_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        PurchasePopup_Info info = new PurchasePopup_Info();
        info.info = item.Name + "을\n해당 가격에 구매 하시겠습니까?";
        info.price = item.Price;
        info.moneyType = 0;
        info.thumbnail = exist_Image.mainTexture;
        info.func = () => {
            int price = int.Parse(item.Price);
            if (StaticManager.Backend.backendGameData.UserData.Ganet >= price)
            {
                var cal = StaticManager.Backend.backendGameData.UserData.Ganet -= price;
                StaticManager.Backend.backendGameData.UserData.SetGanet(cal);
                StaticManager.Backend.backendGameData.UserData.Update((callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        StaticManager.Backend.backendGameData.FriendsData.SetSkin(item.OriginCode,item.Code);
                        StaticManager.Backend.backendGameData.FriendsData.Update((callback) =>
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
                                StaticManager.Backend.backendGameData.FriendsData.Skin[item.OriginCode].Remove(item.Code);
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
                StaticManager.UI.AlertUI.OpenUI("Info", "가넷 갯수가 부족합니다.");
            }
        };

        GameManager.Instance.ShowPurchasePopup(info);
    }

    public void ReSettingFriends()
    {

        if (StaticManager.Backend.backendGameData.FriendsData.Skin.ContainsKey(item.OriginCode))
        {
            state = StaticManager.Backend.backendGameData.FriendsData.Skin[item.OriginCode].Contains(item.Code) ? ContensState.on : ContensState.unPurchase;
        }
        else
        {
            state = ContensState.unPurchase;

        }
        Texture image = Resources.Load<Texture>(item.ResourcePath);
        exist_Image.mainTexture = image;
        purchase_Image.mainTexture = image;
    }
}
