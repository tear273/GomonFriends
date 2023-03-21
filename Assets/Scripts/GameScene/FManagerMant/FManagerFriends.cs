using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FManagerFriends : MonoBehaviour
{
    [SerializeField]
    UIGrid grid;

    [SerializeField]
    GameObject origin_ManageManetFriend;

    public void SetFriendsList()
    {
        List<FriendsChart.Item> list = StaticManager.Chart.Friends.friendsSheet;

        for(int i=0; i<list.Count; i++)
        {
            ManageMentFriend friends = NGUITools.AddChild(grid.gameObject, origin_ManageManetFriend).GetComponent<ManageMentFriend>();
            friends.SetData(list[i]);

        }
        grid.enabled = true;
    }

    public void SetReData()
    {
        ManageMentFriend[] list = GetComponentsInChildren<ManageMentFriend>();
        for (int i = 0; i < list.Length; i++)
        {
            list[i].SetState();
        }
    }
}
