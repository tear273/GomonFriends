using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDeco : MonoBehaviour
{
    [SerializeField]
    private AIFriends friends;

    public AIFriends Friends
    {
        get
        {
            return friends;
        }
        set
        {
            friends = value;
        }
    }
}
