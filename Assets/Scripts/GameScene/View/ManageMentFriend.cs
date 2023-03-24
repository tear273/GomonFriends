using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMentFriend : MonoBehaviour
{
    [SerializeField]
    GameObject onOff;
    [SerializeField]
    GameObject locked;

    [SerializeField]
    UITexture lock_Image;
    [SerializeField]
    UILabel price_Label;
    [SerializeField]
    UILabel lock_name_Lable;
    [SerializeField]
    UILabel lock_subInfo_Label;
    [SerializeField]
    UILabel lock_info_Label;
    [SerializeField]
    UIButton purchase_btn;

    [SerializeField]
    UITexture onOff_Image;
    [SerializeField]
    UILabel onOff_name_Lable;
    [SerializeField]
    UILabel onOff_subInfo_Label;
    [SerializeField]
    UILabel onOff_info_Label;
    [SerializeField]
    UILabel state_Label;
    [SerializeField]
    UIToggle siwth_Toggle;
    [SerializeField]
    UIToggle bg_Toggle;

    public Texture Image
    {
        set
        {
            lock_Image.mainTexture = value;
            onOff_Image.mainTexture = value;
        }
    }

    public string Name
    {
        get
        {
            return lock_name_Lable.text;
        }
        set
        {
            lock_name_Lable.text = value;
            onOff_name_Lable.text = value;
        }
    }

    public string SubInfo
    {
        set
        {
            lock_subInfo_Label.text = value;
            onOff_subInfo_Label.text = value;
        }
    }

    public string Info
    {
        set
        {
            lock_info_Label.text = value;
            onOff_info_Label.text = value;
        }
    }

    public string OnOffstate
    {
        set
        {
            state_Label.text = value;
        }
    }

    public string price
    {
        get
        {
            return price_Label.text;
        }
        set
        {
            price_Label.text = value;
        }
    }

    public ContensState state
    {
        set
        {
            switch (value)
            {
                case ContensState.unPurchase:
                    onOff.SetActive(false);
                    locked.SetActive(true);
                    break;
                case ContensState.on:
                    onOff.SetActive(true);
                    locked.SetActive(false);
                    siwth_Toggle.value = true;
                    bg_Toggle.value = true;
                    break;
                case ContensState.off:
                    onOff.SetActive(true);
                    locked.SetActive(false);
                    siwth_Toggle.value = false;
                    bg_Toggle.value = false;
                    break;
                case ContensState.locked:
                    onOff.SetActive(false);
                    locked.SetActive(true);
                    break;
            }
        }
    }
    FriendsChart.Item item;
    public void SetData(FriendsChart.Item item)
    {
        this.item = item;
        Name = item.Name;
        Info = item.Info;
        SubInfo = item.SubInfo;
        price = item.Price;
        SetState();
    }

    public void SetState()
    {
        Dictionary<string, bool> dic = StaticManager.Backend.backendGameData.FriendsData.Friends;
        if (dic.ContainsKey(item.Code))
        {
            if (dic[item.Code])
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
    }

    private void Start()
    {
        AddLisener();
    }

    void AddLisener()
    {
        EventDelegate _event = new EventDelegate(OnClickPurchase_Btn);
        purchase_btn.onClick.Add(_event);

        _event = new EventDelegate(OnChangeToggle);
        siwth_Toggle.onChange.Add(_event);
    }

    void OnChangeToggle()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        state_Label.text = siwth_Toggle.value ? "초대중 입니다." : "대기중 입니다.";
        if(StaticManager.Backend.backendGameData.FriendsData.Friends[item.Code] != siwth_Toggle.value)
        {
            StaticManager.Backend.backendGameData.FriendsData.SetFriends(item.Code, siwth_Toggle.value);
            StaticManager.Backend.backendGameData.FriendsData.Update((callback) =>
            {
                if (!callback.IsSuccess())
                {
                    
                    StaticManager.Backend.backendGameData.FriendsData.SetFriends(item.Code, !siwth_Toggle.value);
                    siwth_Toggle.value = !siwth_Toggle.value;
                }
                else
                {
                    GameManager.Instance.Friends.Find(obj => obj.name.Equals(item.Code)).SetActive(siwth_Toggle.value);
                    
                }
                
            });
        }
    }

    void OnClickPurchase_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        PurchasePopup_Info info = new PurchasePopup_Info();
        info.info = Name + " 프렌즈를\n해당 가격에 구매 하시겠습니?";
        info.price = price;
        info.moneyType = 1;

        info.func = () => {
            if (StaticManager.Backend.backendGameData.UserData.FriendShipStar >= int.Parse(item.Price) )
            {
                var cal = StaticManager.Backend.backendGameData.UserData.FriendShipStar -= int.Parse(item.Price);
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
                                state = ContensState.off;
                                StaticManager.UI.AlertUI.OpenUI("Info", Name + " 구매 완료!");
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
                        StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(cal + int.Parse(item.Price));
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
