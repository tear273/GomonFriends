using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFriends : MonoBehaviour
{
    [SerializeField]
    UIToggle customFriends_Toggle;

    [SerializeField]
    UILabel name_Label;

    [SerializeField]
    UIButton button;
    [SerializeField]
    UITexture icone;

    FriendsChart.Item item;

    public FriendsChart.Item Item => item;

    public UIButton Button => button;
    UITexture charactor_Img;

    Texture selectImg;

    public void SetItem(FriendsChart.Item item,UITexture charactor_image)
    {
        Name = item.Name;
        this.item = item;
        Texture image = Resources.Load<Texture>(item.FriendsPath);
        selectImg = Resources.Load<Texture>(item.FriendsPurchasePath);
        icone.mainTexture = image;
        charactor_Img = charactor_image;
    }

    public void ChangeToogle()
    {
        if (customFriends_Toggle.value)
        {
            charactor_Img.mainTexture = selectImg;
        }
    }

    public bool CustomFriends_Toggle
    {
        set
        {
            customFriends_Toggle.value = value;
            
        }
    }

    public string Name
    {
        set
        {
            name_Label.text = value;
        }
    }
}
