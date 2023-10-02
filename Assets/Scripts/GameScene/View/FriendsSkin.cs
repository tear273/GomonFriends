using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;

public class FriendsSkin : MonoBehaviour
{
    [SerializeField]
    UITexture image;

    [SerializeField]
    UIToggle toggle;

    public bool Toggle
    {
        set
        {
            toggle.value = value;
        }
    }

    public Texture Image
    {
        set
        {
            image.mainTexture = value;
        }
    }
    SkinChart.Item item;
    UITexture charactor_Img;

    public void SetItem(SkinChart.Item item, UITexture charactor_image)
    {
        this.item = item;
        charactor_Img = charactor_image;
        image.mainTexture = Resources.Load(item.ResourcePath) as Texture;
    }

    void AddEvent()
    {
        UIEventTrigger trigger = gameObject.AddComponent<UIEventTrigger>();

        EventDelegate _event = new EventDelegate(OnClickSkin);
        trigger.onClick.Add(_event);

        _event = new EventDelegate(OnChangeToggle);
        toggle.onChange.Add(_event);
    }

    void OnClickSkin()
    {
        
        StaticManager.Backend.backendGameData.FriendsData.SetConnectSkin(item.OriginCode,item.Code);
        StaticManager.Backend.backendGameData.FriendsData.Update((callback) =>
        {
            if (callback.IsSuccess())
            {
                Transform friends = GameObject.Find(item.OriginCode).transform;

                for(int i=0; i<friends.childCount; i++)
                {
                    bool check = friends.GetChild(i).gameObject.name == item.Code;
                    if (check)
                    {
                        friends.GetComponent<AIFriends>().animator = friends.GetChild(i).GetComponent<Animator>();
                    }
                    friends.GetChild(i).gameObject.SetActive(check);
                }

                charactor_Img.mainTexture = Resources.Load(item.ResourcePath) as Texture;
            }
            else
            {
               // StaticManager.Backend.backendGameData.UserData.SetGanet(cal + price);
            }

        });
    }

    private void Start()
    {
        AddEvent();
    }

    void OnChangeToggle()
    {
        if (toggle.value)
            StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
    }
}
