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

    FriendsChart.Item item;

    public FriendsChart.Item Item => item;

    public UIButton Button => button;

    public void SetItem(FriendsChart.Item item)
    {
        Name = item.Name;
        this.item = item;
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
