using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FManagerManet_View : MonoBehaviour
{
    [SerializeField]
    UIButton close_btn;

    [SerializeField]
    FManagerFriends friendsManagerFriends;
    [SerializeField]
    FManagerCustom friendsManagerCustom;

    [SerializeField]
    UIToggle friends_Toggle;
    [SerializeField]
    UIToggle custom_Toggle;

    public Action func;

    private void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        AddEvents();
        SetFManagerFriends();
    }

    void SetFManagerFriends()
    {
        friendsManagerFriends.SetFriendsList();
    }

    private void OnEnable()
    {
        friendsManagerFriends.SetReData();
    }

    void AddEvents()
    {
        EventDelegate _event = new EventDelegate(OnClickCloase_Btn);
        close_btn.onClick.Add(_event);

         _event = new EventDelegate(OnChangeFriends_Toggle);
        friends_Toggle.onChange.Add(_event);

         _event = new EventDelegate(OnChangeCustom_Toggle);
        custom_Toggle.onChange.Add(_event);
    }

    void OnChangeFriends_Toggle()
    {
        if (friends_Toggle.value)
        {
            StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        }
    }

    void OnChangeCustom_Toggle()
    {
        if (custom_Toggle.value)
        {
            StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        }
    }

    void OnClickCloase_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        func();
        gameObject.SetActive(false);
    }
}
