using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common_Popup : MonoBehaviour
{
    [SerializeField]
    UILabel title_Label;
    [SerializeField]
    UILabel note_Label;
    [SerializeField]
    UIButton confirem_btn;
    Action func;

    // Start is called before the first frame update
    void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        
        AddLisener();
    }

    void AddLisener()
    {
        EventDelegate _event = new EventDelegate(OnClickConfirem_Btn);
        confirem_btn.onClick.Add(_event);
    }

    void OnClickConfirem_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        if(func != null)
        {
            func();
        }
        gameObject.SetActive(false);
    }

    public void OpenUI(string title, string note,Action func = null)
    {
        title_Label.text = title;
        note_Label.text = note;
        this.func = func;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        NGUITools.BringForward(gameObject);
    }
}
