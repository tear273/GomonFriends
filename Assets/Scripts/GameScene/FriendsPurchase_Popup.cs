using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsPurchase_Popup : MonoBehaviour
{
    [SerializeField]
    UILabel price_Label;

    [SerializeField]
    UITexture image;

    [SerializeField]
    UIButton cancel_btn;

    [SerializeField]
    UIButton confirem_btn;

    [SerializeField]
    UILabel info_Label;

    [SerializeField]
    PurhcaseType purchaseType;

    [SerializeField]
    Texture ganet;

    [SerializeField]
    Texture FriendsShipStar;

    [SerializeField]
    UITexture moneyType_Texture;

    public Action func;
    [SerializeField]
    string code;
    int moneyType;

    public int MoneyType
    {
        set
        {
            switch (value)
            {
                case 0:
                    moneyType_Texture.mainTexture = ganet;
                    break;
                case 1:
                    moneyType_Texture.mainTexture = FriendsShipStar;
                    break;
            }
            moneyType = value;
        }
    }

    public string Code
    {
        get
        {
            return code;
        }
        set
        {
            code = value;
        }
    }


    public PurhcaseType Type
    {
        get
        {
            return purchaseType;
        }
        set
        {
            purchaseType = value;
        }
    }

    public string Price
    {
        set
        {
            price_Label.text = value;
        }
    }

    public Texture Image
    {
        set
        {
            image.mainTexture = value;
        }
    }

    public string Info
    {
        set
        {
            info_Label.text = value;
        }
    }

    private void Start()
    {
        AddLisener();
    }

    void AddLisener()
    {
        EventDelegate _event = new EventDelegate(OnClickCancel_Btn);
        cancel_btn.onClick.Add(_event);

        _event = new EventDelegate(OnClickConfirem_Btn);
        confirem_btn.onClick.Add(_event);
    }

    void OnClickCancel_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        gameObject.SetActive(false);
    }

    void OnClickConfirem_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        if (func != null)
        {
            func();
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        
    }


    private void OnDisable()
    {
        func = null;
    }


    private void OnEnable()
    {
        NGUITools.BringForward(gameObject);
    }
}
