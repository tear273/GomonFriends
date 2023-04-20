using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Friends_Panel : MonoBehaviour
{
    [SerializeField]
    UIGrid friend_grid;

    [SerializeField]
    GameObject origin_store_Friends;

    private void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        SetFriendsList();
    }

    public void SetFriendsList()
    {
        List<FriendsChart.Item> list = StaticManager.Chart.Friends.friendsSheet;

        for (int i = 0; i < list.Count; i++)
        {
            Store_Friends friends = NGUITools.AddChild(friend_grid.gameObject, origin_store_Friends).GetComponent<Store_Friends>();
            friends.SetData(list[i]);

        }
        friend_grid.enabled = true;
    }
}
