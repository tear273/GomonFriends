using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friends_View : MonoBehaviour
{
    [SerializeField]
    GameObject _OriginInviteFriends;
    [SerializeField]
    UIScrollView inviteFriends_ScrollView;
    [SerializeField]
    GameObject leftGrid;
    [SerializeField]
    GameObject rightGrid;

    [SerializeField]
    UIButton close_btn;

    [SerializeField]
    UIWidget inviteFriendsScroll_Widget;
    [SerializeField]
    UIWidget inviteFriends_Widget;

    [SerializeField]
    UIButton friendsManagement_btn;

    int friendsGap = 10;
    int friendsHeight = 70;

    private void Start()
    {
        Initalized();
        SetFriends();
    }

    void SetFriends()
    {
        NGUITools.DestroyChildren(leftGrid.transform);
        NGUITools.DestroyChildren(rightGrid.transform);

        Dictionary<string, bool> dic = StaticManager.Backend.backendGameData.FriendsData.Friends;
        List<FriendsChart.Item> friends = StaticManager.Chart.Friends.friendsSheet;

        int i = 0;
        foreach(string key in dic.Keys)
        {
            InviteFriends _friends = NGUITools.AddChild(i % 2 == 0 ? leftGrid : rightGrid, _OriginInviteFriends).GetComponent<InviteFriends>();
            _friends.SetData(friends.Find(item => item.Code.Equals(key)), dic[key]);
            if (StaticManager.UI.currState == CurrState.TUTORIAL && i == 0)
            {
                GameManager.Instance.Tutorials.fifthFunc = () =>
                {
                    _friends.toggle.value = true;
                };
            }
            i ++;
        }

        int height = 0;

        if (dic.Count % 2 == 0)
        {
            height = dic.Count / 2 * (friendsGap + friendsHeight);
        }
        else
        {
            height = (dic.Count + 1) / 2 * (friendsGap + friendsHeight);
        }

        

        inviteFriends_Widget.height = height > 403 ? 403 : height + 81;
        inviteFriendsScroll_Widget.height = height > 403 ? 403 : height;

        leftGrid.GetComponent<UIGrid>().enabled = true;
        rightGrid.GetComponent<UIGrid>().enabled = true;

        
    }

    void Initalized()
    {
        AddLisener();
        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForEndOfFrame();
        NGUITools.BringForward(GameManager.Instance.Tutorials.gameObject);
    }

    void AddLisener()
    {
        EventDelegate _event = new EventDelegate(OnClickClose_Btn);
        close_btn.onClick.Add(_event);

        _event = new EventDelegate(OnClickFriendsManagement_Btn);
        friendsManagement_btn.onClick.Add(_event);
    }

    void OnClickFriendsManagement_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        GameManager.Instance.FrendsManageMent_View.func = () =>
        {
            SetFriends();
        };

        GameManager.Instance.FrendsManageMent_View.gameObject.SetActive(true);
    }

    public void OnClickClose_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        gameObject.SetActive(false);
    }

}
