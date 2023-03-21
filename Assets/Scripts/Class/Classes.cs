using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InviteFriends_Info
{
    public string name;
    public bool check;
    public Texture thumnail;
}

public enum ContensState
{
    unPurchase,
    on,
    off,
    locked
}

public enum PurhcaseType
{
    Deco,
    Friends
}

public class FriendsManagementFriend_Info
{
    public Texture image;
    public string name;
    public string subInfo;
    public string info;
    public string price;
    public ContensState state;
    
}

public class CustomFriends_Info
{
    public Texture thumbnail;
    public string name;
}


public class FriendsSkin_Info
{
    public Texture thumbnail;
}

public class FriendsAnim_Info
{
    public Texture thumbnail;
}

public class PurchasePopup_Info
{
    public Texture thumbnail;
    public string info;
    public string price;
    public int moneyType;
    public Action func;
}

