using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FManagerCustom : MonoBehaviour
{
    [SerializeField]
    GameObject _Origin_customFriends;

    [SerializeField]
    GameObject _Origin_FriendSkin;

    [SerializeField]
    GameObject _Origin_FriendAnim;

    [SerializeField]
    UIGrid friendsGrid;

    [SerializeField]
    UIGrid skinGrid;

    [SerializeField]
    UIGrid animGrid;
    [SerializeField]
    UITexture charector_img;

    private void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        SetFriends();
        FriendsAnimTest();
    }

   

    void SetFriends() {
        List<FriendsChart.Item> list = StaticManager.Chart.Friends.friendsSheet;

        for(int i=0; i<list.Count; i++)
        {
            CustomFriends custom = NGUITools.AddChild(friendsGrid.gameObject, _Origin_customFriends).GetComponent<CustomFriends>();
            custom.SetItem(list[i],charector_img);

            if (i == 0)
            {
                custom.CustomFriends_Toggle = true;
            }

            EventDelegate _event = new EventDelegate(() =>
            {
                StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
                SetSkin(custom.Item.Skins);
            });
            custom.Button.onClick.Add(_event);
        }

        friendsGrid.enabled = true;

    }

    void SetSkin(string[] items)
    {
        NGUITools.DestroyChildren(skinGrid.transform);
        for (int i=0; i<items.Length; i++)
        {
            FriendsSkin skin = NGUITools.AddChild(skinGrid.gameObject, _Origin_FriendSkin).GetComponent<FriendsSkin>();
            if (i == 0)
            {
                skin.Toggle = true;
            }
        }
        skinGrid.enabled = true;
    }

    void FriendsAnimTest()
    {
        List<FriendsAnim_Info> infos = new List<FriendsAnim_Info>();

        for (int i = 0; i < 5; i++)
        {
            FriendsAnim_Info info = new FriendsAnim_Info();

            infos.Add(info);

        }


        SetAnim(infos);
    }

    void SetAnim(List<FriendsAnim_Info> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            FriendsAnim skin = NGUITools.AddChild(animGrid.gameObject, _Origin_FriendAnim).GetComponent<FriendsAnim>();
            if (i == 0)
            {
                skin.Toggle = true;
            }
        }
        animGrid.enabled = true;
    }

   
}
