using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Box : MonoBehaviour
{
    [SerializeField]
    GameObject purchase;

    [SerializeField]
    GameObject free;

    [SerializeField]
    GameObject exit;

    [SerializeField]
    UILabel purchaseName_Label;

    [SerializeField]
    UILabel purchaseNum_Label;

    [SerializeField]
    UITexture purchaseIcone_Texture;

    [SerializeField]
    UIButton purchase_btn;

    [SerializeField]
    UILabel freeTitle_Label;

    [SerializeField]
    UIButton free_btn;

    [SerializeField]
    UILabel possibleNumber_Label;

    BoxChart.Item item;

    public void SetData(BoxChart.Item item)
    {
        this.item = item;
        Button_Type = item.Btn_Type;
        freeTitle_Label.text = "[9DA0A7]" + item.Title + "[-]" + "[FFFFFF]" + item.Num + "[-]" + "[9DA0A7]개[-]";

        SetPossibleNumberText(item.PossibleNumber - StaticManager.Backend.backendGameData.PurchaseData.GetFreeNum(item.Code));

    }

    void SetPossibleNumberText(int num)
    {
        possibleNumber_Label.text = "[9DA0A7]" + "오늘 남은 횟수 : " + "[-]" + "[FFFFFF]" + num + "[-]";
    }

    private void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        AddOnClick();
    }

    void AddOnClick()
    {
        EventDelegate _evnet = new EventDelegate(OnClickFree_Button);
        free_btn.onClick.Add(_evnet);
    }

    void OnClickFree_Button()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        var currNum = item.PossibleNumber - StaticManager.Backend.backendGameData.PurchaseData.GetFreeNum(item.Code);
        if(currNum > 0)
        {
            StaticManager.Ads.ShowAd(() =>
            {
                switch (item.Gift_Type)
                {
                    case 0:
                        int ganet = StaticManager.Backend.backendGameData.UserData.Ganet + item.Num;
                        StaticManager.Backend.backendGameData.UserData.SetGanet(ganet);
                        StaticManager.Backend.backendGameData.UserData.Update((callback) => {
                            if (callback.IsSuccess())
                            {
                                StaticManager.UI.AlertUI.OpenUI("Info", "가넷 " + item.Num + "개를 얻었습니다.");
                                GameManager.Instance.Ganet_Label.text = ganet.ToString();
                            }
                        });
                        break;
                    case 1:

                        int star = StaticManager.Backend.backendGameData.UserData.FriendShipStar + item.Num;
                        StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(star);
                        StaticManager.Backend.backendGameData.UserData.Update((callback) => {
                            if (callback.IsSuccess())
                            {
                                StaticManager.UI.AlertUI.OpenUI("Info", "우정별 " + item.Num + "개를 얻었습니다.");
                                GameManager.Instance.FriendsShip_Label.text = star.ToString();
                            }
                        });
                        break;
                }
                int num = StaticManager.Backend.backendGameData.PurchaseData.GetFreeNum(item.Code) + 1;
                StaticManager.Backend.backendGameData.PurchaseData.SetFreeNum(item.Code, num);
                StaticManager.Backend.backendGameData.PurchaseData.Update((callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        SetPossibleNumberText(item.PossibleNumber - num);
                    }
                });


                
            });
        }
        else
        {
            StaticManager.UI.AlertUI.OpenUI("Info", "남은 횟수가 없습니다.");
        }
       
    }

    int Button_Type
    {
        set
        {
            switch (value)
            {
                case 0:
                    exit.SetActive(false);
                    purchase.SetActive(false);
                    free.SetActive(true);
                    break;
                case 1:
                    exit.SetActive(false);
                    purchase.SetActive(true);
                    free.SetActive(false);
                    break;
            }
        }
    }
}
