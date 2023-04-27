using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Popup : MonoBehaviour
{
    [SerializeField]
    UIButton close_btn;

    [SerializeField]
    UIToggle friends;

    private void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        AddListener();
        if(StaticManager.UI.currState == CurrState.TUTORIAL)
        {
            StartCoroutine(ShowFriends());
            
        }
    }

    IEnumerator ShowFriends()
    {
        yield return new WaitForEndOfFrame();
        friends.value = true;
        yield return new WaitForEndOfFrame();
        NGUITools.BringForward(GameManager.Instance.Tutorials.gameObject);
    }

    void AddListener()
    {
        EventDelegate _event = new EventDelegate(OnClickClose_Btn);
        close_btn.onClick.Add(_event);
    }

    public void OnClickClose_Btn()
    {
        gameObject.SetActive(false);
    }
}
