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
       // SetFriends();
        FriendsAnimTest();
    }

    public void ReSetting()
    {
        SetFriends();
    }



    void SetFriends() {

        friendsGrid.transform.DestroyChildren();
        List<FriendsChart.Item> list = StaticManager.Chart.Friends.friendsSheet;
        Dictionary<string, bool> purchase = StaticManager.Backend.backendGameData.FriendsData.Friends;

        for(int i=0; i<list.Count; i++)
        {
            if (purchase.ContainsKey(list[i].Code))
            {
                CustomFriends custom = NGUITools.AddChild(friendsGrid.gameObject, _Origin_customFriends).GetComponent<CustomFriends>();
                custom.SetItem(list[i], charector_img);

                if (i == 0)
                {
                    custom.CustomFriends_Toggle = true;
                    FilterSkin(custom);

                }

                EventDelegate _event = new EventDelegate(() =>
                {
                    StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
                    FilterSkin(custom);
                });
                custom.Button.onClick.Add(_event);
            }
           
        }

        friendsGrid.enabled = true;

    }

    void FilterSkin(CustomFriends custom)
    {
        List<SkinChart.Item> items = StaticManager.Chart.SkinChart.skinSheet;
        Dictionary<string, List<string>> skinData = StaticManager.Backend.backendGameData.FriendsData.Skin;
        List<SkinChart.Item> realItem = new List<SkinChart.Item>();

        realItem.Add(items.Find(x => x.Code == (custom.Item.Code + "_0")));

        if (skinData.ContainsKey(custom.Item.Code))
        {
            List<string> skins = skinData[custom.Item.Code];
            for (int j = 0; j < skins.Count; j++)
            {
                realItem.Add(items.Find(x => x.Code == skins[j]));
            }
        }

        SetSkin(realItem);
    }

    void SetSkin(List<SkinChart.Item> items)
    {
        NGUITools.DestroyChildren(skinGrid.transform);
        Dictionary<string, string> conSkins = StaticManager.Backend.backendGameData.FriendsData.ConnectSkin;
        for (int i=0; i<items.Count; i++)
        {
            FriendsSkin skin = NGUITools.AddChild(skinGrid.gameObject, _Origin_FriendSkin).GetComponent<FriendsSkin>();
            skin.SetItem(items[i], charector_img);


            if (conSkins.ContainsKey(items[i].OriginCode))
            {
                if(conSkins[items[i].OriginCode] == items[i].Code)
                {
                    skin.Toggle = true;
                }
            }
            else
            {
                if (i == 0)
                {
                    skin.Toggle = true;
                }
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
