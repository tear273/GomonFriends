using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question_Popup : MonoBehaviour
{
    [SerializeField]
    UIButton cancel_btn;
    [SerializeField]
    UIButton confirem_btn;
    [SerializeField]
    UILabel note_label;

    private void Start()
    {
        Initalize();
    }

    void Initalize()
    {
        EventDelegate _event = new EventDelegate(OnClickCancel_Btn);
        cancel_btn.onClick.Add(_event);
    }

    public void Init(EventDelegate.Callback func,string note)
    {
        confirem_btn.onClick.Clear();

        EventDelegate _event = new EventDelegate(func);
        confirem_btn.onClick.Add(_event);

        note_label.text = note;
    }

    void OnClickCancel_Btn()
    {
        gameObject.SetActive(false);
    }
}
