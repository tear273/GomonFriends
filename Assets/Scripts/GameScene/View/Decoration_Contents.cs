using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration_Contents : MonoBehaviour
{
    [SerializeField]
    GameObject purchase;
    [SerializeField]
    GameObject onOff;

    [SerializeField]
    UITexture purchase_Image;
    [SerializeField]
    UILabel purchse_Name_Label;
    [SerializeField]
    UILabel purchse_Note_Label;
    [SerializeField]
    UILabel purchse_Price_Label;
    [SerializeField]
    UIButton purchase_btn;
    [SerializeField]
    UITexture priceType_Texture;

    [SerializeField]
    UITexture onOff_Image;
    [SerializeField]
    UILabel onOff_Name_Label;
    [SerializeField]
    UILabel onOff_Note_Label;
    [SerializeField]
    UIToggle switch_Toggle;
    [SerializeField]
    UIToggle border_Toggle;

    [SerializeField]
    Texture frendship;
    [SerializeField]
    Texture ganet;

    string decoCode = "";
    int priceFrom = 0;
    int price = 0;

    public void SetData(DecoChart.Item item)
    {
        Name = item.Name;
        Price = item.Price.ToString();
        Note = item.Info.Replace("\\n", "\n");
        Image = (Texture)Resources.Load("UI/Deco/" + item.TextrueUrl);
        if (StaticManager.Backend.backendGameData.DecoData.Deco.ContainsKey(item.Code))
        {
            if (StaticManager.Backend.backendGameData.DecoData.Deco[item.Code])
            {
                state = ContensState.on;
            }
            else
            {
                state = ContensState.off;
            }
        }
        else
        {
            state = ContensState.unPurchase;
        }

        
        DecoCode = item.Code;
        PriceFrom = item.From;
        price = item.Price;
        

    }

    public string DecoCode
    {
        get
        {
            return decoCode;
        }
        set
        {
            decoCode = value;
        }
    }

    public int PriceFrom
    {
        get
        {
            return priceFrom;
        }
        set
        {
            priceFrom = value;
            priceType_Texture.mainTexture = priceFrom == 1 ? frendship : ganet;

        }
    }

    public Texture Image
    {
        get
        {
            return purchase_Image.mainTexture;
        }
        set
        {
            purchase_Image.mainTexture = value;
            onOff_Image.mainTexture = value;
        }
    }

    public string Name
    {
        get
        {
            return onOff_Name_Label.text;
        }
        set
        {
            onOff_Name_Label.text = value;
            purchse_Name_Label.text = value;
        }
    }

    public string Price
    {
        get
        {
            return purchse_Price_Label.text;
        }
        set
        {
            purchse_Price_Label.text = value;
        }
    }

    public string Note
    {
        set
        {
            purchse_Note_Label.text = value;
            onOff_Note_Label.text = value;
        }
    }

    public bool Toggle
    {
        set
        {
            switch_Toggle.value = value;
            border_Toggle.value = value;
        }
    }

    public ContensState state
    {
        set
        {
            switch(value)
            {
                case ContensState.unPurchase:
                    purchase.SetActive(true);
                    onOff.SetActive(false);
                    break;
                case ContensState.off:
                    purchase.SetActive(false);
                    onOff.SetActive(true);
                    Toggle = false;
                    break;
                case ContensState.on:
                    purchase.SetActive(false);
                    onOff.SetActive(true);
                    Toggle = true;
                    break;
            }
        }
    }

    public void OnClickSwitch()
    {
        StaticManager.Backend.backendGameData.DecoData.SetDeco(DecoCode,switch_Toggle.value);
    }


    private void Start()
    {
        AddListener();
    }

    void AddListener()
    {
        EventDelegate _event = new EventDelegate(OnClickPurchase_Btn);
        purchase_btn.onClick.Add(_event);
    }

    void OnClickPurchase_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        PurchasePopup_Info info = new PurchasePopup_Info();
        info.info = Name + "을\n해당 가격에 구매 하시겠습니까?";
        info.price = Price;
        info.moneyType = priceFrom;
        info.thumbnail = Image;
        info.func = () => {
            switch (PriceFrom)
            {
                case 0:
                    if (StaticManager.Backend.backendGameData.UserData.Ganet >= price)
                    {
                        var cal = StaticManager.Backend.backendGameData.UserData.Ganet -= price;
                        StaticManager.Backend.backendGameData.UserData.SetGanet(cal);
                        StaticManager.Backend.backendGameData.UserData.Update((callback) =>
                        {
                            if (callback.IsSuccess())
                            {
                                StaticManager.Backend.backendGameData.DecoData.SetDeco(DecoCode);
                                StaticManager.Backend.backendGameData.DecoData.Update((callback) =>
                                {
                                    if (callback.IsSuccess())
                                    {
                                        GameManager.Instance.Ganet_Label.text = StaticManager.Backend.backendGameData.UserData.Ganet.ToString();
                                        state = ContensState.off;
                                        StaticManager.UI.AlertUI.OpenUI("Info", Name + " 구매 완료!");
                                        GameManager.Instance.FriendsPurchase_Popup.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        StaticManager.Backend.backendGameData.DecoData.Deco.Remove(DecoCode);
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
                    break;
                    
                case 1:
                    if (StaticManager.Backend.backendGameData.UserData.FriendShipStar >= price)
                    {
                        var cal = StaticManager.Backend.backendGameData.UserData.FriendShipStar -= price;
                        StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(cal);
                        StaticManager.Backend.backendGameData.UserData.Update((callback) =>
                        {
                            if (callback.IsSuccess())
                            {
                                StaticManager.Backend.backendGameData.DecoData.SetDeco(DecoCode);
                                StaticManager.Backend.backendGameData.DecoData.Update((callback) =>
                                {
                                    if (callback.IsSuccess())
                                    {
                                        GameManager.Instance.FriendsShip_Label.text = StaticManager.Backend.backendGameData.UserData.FriendShipStar.ToString();
                                        state = ContensState.off;
                                        StaticManager.UI.AlertUI.OpenUI("Info", Name + " 구매 완료!");
                                        GameManager.Instance.FriendsPurchase_Popup.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        StaticManager.Backend.backendGameData.DecoData.Deco.Remove(DecoCode);
                                    }
                                });
                            }
                            else
                            {
                                StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(cal + price);
                            }

                        });

                    }
                    else
                    {
                        StaticManager.UI.AlertUI.OpenUI("Info", "우정별 갯수가 부족합니다.");
                    }



                    if (StaticManager.Backend.backendGameData.UserData.FriendShipStar > price)
                    {
                        GameManager.Instance.CalFriendShipStar(-price);
                        StaticManager.Backend.backendGameData.DecoData.SetDeco(DecoCode);
                        state = ContensState.off;
                        StaticManager.UI.AlertUI.OpenUI("Info", Name + " 구매 완료!");
                        //GameManager.Instance.FriendsPurchase_Popup.gameObject.SetActive(false);

                    }
                    else
                    {
                        StaticManager.UI.AlertUI.OpenUI("Info", "우정별 갯수가 부족합니다.");
                    }
                    break;
            }
        };

        GameManager.Instance.ShowPurchasePopup(info);
    }
}
