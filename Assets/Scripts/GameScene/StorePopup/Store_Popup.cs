using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Popup : MonoBehaviour
{
    [SerializeField]
    UIButton close_btn;

    private void Start()
    {
        Initalized();
    }

    void Initalized()
    {
        AddListener();
    }

    void AddListener()
    {
        EventDelegate _event = new EventDelegate(OnClickClose_Btn);
        close_btn.onClick.Add(_event);
    }

    void OnClickClose_Btn()
    {
        gameObject.SetActive(false);
    }
}
