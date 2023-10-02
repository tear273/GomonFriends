using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Friends_Panel : MonoBehaviour
{
    [SerializeField]
    UIGrid friend_grid;

    [SerializeField]
    GameObject origin_store_Friends;

    [SerializeField]
    UIGrid skin_grid;

    [SerializeField]
    GameObject origin_store_Skin;

    List<Store_Friends> friends = new List<Store_Friends>();
    List<Store_Skin> skin = new List<Store_Skin>();
    private void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        SetFriendsList();
        setSkinLIst();
    }

    void setSkinLIst()
    {
        List<SkinChart.Item> list = StaticManager.Chart.SkinChart.skinSheet;
        print("SkinCount : " + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].Code.Contains("_0"))
            {
                Store_Skin skin = NGUITools.AddChild(skin_grid.gameObject, origin_store_Skin).GetComponent<Store_Skin>();
                skin.SetData(list[i]);

                this.skin.Add(skin);
            }
           

        }
        skin_grid.enabled = true;
    }

    public void SetFriendsList()
    {
        List<FriendsChart.Item> list = StaticManager.Chart.Friends.friendsSheet;

        for (int i = 0; i < list.Count; i++)
        {
            Store_Friends friends = NGUITools.AddChild(friend_grid.gameObject, origin_store_Friends).GetComponent<Store_Friends>();
            friends.SetData(list[i]);
            if(StaticManager.UI.currState == CurrState.TUTORIAL && i == 0)
            {
                GameManager.Instance.Tutorials.fourthFunc = friends.OnClickPurchase_Btn;
            }

            this.friends.Add(friends);

        }
        friend_grid.enabled = true;
    }

    public void ReState()
    {
        for (int i = 0; i < friends.Count; i++)
        {
            friends[i].ReSettingFriends();
        }

        for (int i = 0; i < skin.Count; i++)
        {
            skin[i].ReSettingFriends();
        }
    }
}
