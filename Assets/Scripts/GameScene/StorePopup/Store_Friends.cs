using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Friends : MonoBehaviour
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
    UIButton purchase_btn;

    FriendsChart.Item item;

    public void SetData(FriendsChart.Item item)
    {
        this.item = item;

        exit_Name_Label.text = item.Name;
        purchase_Name_Label.text = item.Name;
        purchase_Price_Label.text = item.Price;
        state = StaticManager.Backend.backendGameData.FriendsData.Friends.ContainsKey(item.Code) ? ContensState.on : ContensState.unPurchase;
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
        PurchasePopup_Info info = new PurchasePopup_Info();
        info.info = item.Name + "을\n해당 가격에 구매 하시겠습니까?";
        info.price = item.Price;
        info.moneyType = 1;
        //info.thumbnail = Image;
        info.func = () => {
            int price = int.Parse(item.Price);
            if (StaticManager.Backend.backendGameData.UserData.FriendShipStar >= price)
            {
                var cal = StaticManager.Backend.backendGameData.UserData.FriendShipStar -= price;
                StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(cal);
                StaticManager.Backend.backendGameData.UserData.Update((callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        StaticManager.Backend.backendGameData.FriendsData.SetFriends(item.Code);
                        StaticManager.Backend.backendGameData.FriendsData.Update((callback) =>
                        {
                            if (callback.IsSuccess())
                            {
                                GameManager.Instance.FriendsShip_Label.text = StaticManager.Backend.backendGameData.UserData.FriendShipStar.ToString();
                                state = ContensState.on;
                                if(StaticManager.UI.currState == CurrState.TUTORIAL)
                                {
                                    StaticManager.UI.AlertUI.OpenUI("Info", item.Name + " 구매 완료!",() =>
                                    {
                                        int num = GameManager.Instance.Tutorials.currNum;
                                        GameManager.Instance.Tutorials.ShowTutorial(num);
                                    });
                                }
                                else
                                {
                                    StaticManager.UI.AlertUI.OpenUI("Info", item.Name + " 구매 완료!");
                                }
                                
                                GameManager.Instance.FriendsPurchase_Popup.gameObject.SetActive(false);
                            }
                            else
                            {
                                StaticManager.Backend.backendGameData.FriendsData.Friends.Remove(item.Code);
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
        };

        GameManager.Instance.ShowPurchasePopup(info);
    }
}
