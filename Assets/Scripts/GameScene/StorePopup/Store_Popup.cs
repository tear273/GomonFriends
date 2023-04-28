using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Popup : MonoBehaviour
{
    [SerializeField]
    UIButton close_btn;

    [SerializeField]
    UIToggle friends;

    [SerializeField]
    GameObject[] bottom_Items;
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

    void PlayButtonsound()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
    }

    void AddListener()
    {
        EventDelegate _event = new EventDelegate(OnClickClose_Btn);
        close_btn.onClick.Add(_event);

        for (int i = 0; i < bottom_Items.Length; i++)
        {
            UIEventTrigger trigger = bottom_Items[i].AddComponent<UIEventTrigger>();
             _event = new EventDelegate(PlayButtonsound);

            trigger.onClick.Add(_event);
        }
    }

    public void OnClickClose_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        gameObject.SetActive(false);
    }
}
