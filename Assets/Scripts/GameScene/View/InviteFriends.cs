using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InviteFriends : MonoBehaviour
{
    [SerializeField]
    UILabel name_Label;

    [SerializeField]
    UILabel on_Name_Label;

    //[SerializeField]
    public UIToggle toggle;

    [SerializeField]
    UITexture thumbNail;

    public UILabel GetNameLabel => name_Label;
    public UIToggle GetToggle => toggle;
    public UITexture GetThumbNail => thumbNail;
    FriendsChart.Item item;

    public void SetData(FriendsChart.Item item, bool IsOn)
    {
        SetName_Label = item.Name;
        SetToggle = IsOn;
        this.item = item;
        Texture image = Resources.Load<Texture>(item.FriendsPath);
        thumbNail.mainTexture = image;
    }

    public string SetName_Label
    {
        set
        {
            name_Label.text = value;
            on_Name_Label.text = value;
        }
    }

    public bool SetToggle
    {
        set
        {
            toggle.value = value;
        }
    }

    public Texture SetThumbNail
    {
        set
        {
            thumbNail.mainTexture = value;
        }
    }

    private void Start()
    {
        EventDelegate _event = new EventDelegate(OnChangeValueToggle);
        toggle.onChange.Add(_event);

        _event = new EventDelegate(OnClick_Btn);
        gameObject.AddComponent<UIEventTrigger>().onClick.Add(_event);
    }

    void OnClick_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
    }

    public void OnChangeValueToggle()
    {
        
        StaticManager.Backend.backendGameData.FriendsData.SetFriends(item.Code, toggle.value);
        StaticManager.Backend.backendGameData.FriendsData.Update((callback) =>
        {
            if (callback.IsSuccess())
            {
                GameManager.Instance.Friends.Find(obj => obj.name.Equals(item.Code)).SetActive(toggle.value);
                GameManager.Instance.SetFrindsNum();
            }
            else
            {
                Debug.LogError("네트워크에러");
                toggle.value = !toggle.value;
            }
        });
    }
}
