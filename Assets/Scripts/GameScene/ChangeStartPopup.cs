using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStartPopup : MonoBehaviour
{

    [SerializeField]
    UISlider slider_Slider;
    [SerializeField]
    UIToggle check_Toggle;
    [SerializeField]
    UILabel starNum_Label;
    [SerializeField]
    UIButton cancel_Btn;
    [SerializeField]
    UIButton confirem_Btn;

    private int walkCount = 0;
    private int max_FirendStar = 0;
    public int WalkCount
    {
        get
        {
            return walkCount;
        }
        set
        {
            walkCount = value;
            max_FirendStar = (int)Mathf.Floor(walkCount) / 10;
            starNum_Label.text = ((int)(Mathf.Floor(max_FirendStar * slider_Slider.value))).ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AddLisener();
    }

    void AddLisener()
    {
        EventDelegate _event = new EventDelegate(OnClickCancel_Btn);
        cancel_Btn.onClick.Add(_event);

        _event = new EventDelegate(OnClickConfirem_Btn);
        confirem_Btn.onClick.Add(_event);

        _event = new EventDelegate(OnSlider);
        slider_Slider.onChange.Add(_event);

        _event = new EventDelegate(OnClickCheck_Btn);
        check_Toggle.onChange.Add(_event);
    }

    void OnClickCheck_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        if (check_Toggle.value)
        {
            slider_Slider.value = 1;
        }
    }

    void OnSlider()
    {
        starNum_Label.text = ((int)(Mathf.Floor(max_FirendStar * slider_Slider.value))).ToString();
        if (slider_Slider.value == 1)
        {
            check_Toggle.value = true;
        }
        else
        {
            check_Toggle.value = false;
        }
    }

    void OnClickCancel_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        gameObject.SetActive(false);
    }

    void OnClickConfirem_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        int purchaseStar = int.Parse(starNum_Label.text);

        StaticManager.Backend.backendGameData.UserData.SetPurchaseFriendShipStar(purchaseStar * 10);
        StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(StaticManager.Backend.backendGameData.UserData.FriendShipStar + purchaseStar);
        StaticManager.Backend.backendGameData.UserData.Update((callback) =>
        {
            if (callback.IsSuccess())
            {
                GameManager.Instance.PedometerPlugin.LoadTotalStep();
                GameManager.Instance.FriendsShip_Label.text = StaticManager.Backend.backendGameData.UserData.FriendShipStar.ToString();
                StaticManager.UI.alertUI.OpenUI("Info", "우정별 " + purchaseStar + "개 구매 하셨습니다.");
            }
            else
            {
                Debug.LogError("우정별 구매 실패");
                StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(StaticManager.Backend.backendGameData.UserData.FriendShipStar - purchaseStar);
                StaticManager.Backend.backendGameData.UserData.SetPurchaseFriendShipStar(-purchaseStar * 10);
            }

        });

        gameObject.SetActive(false);
    }

    
}
