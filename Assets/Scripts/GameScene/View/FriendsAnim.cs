using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsAnim : MonoBehaviour
{
    [SerializeField]
    UITexture image;

    [SerializeField]
    UIToggle toggle;

    public bool Toggle
    {
        set
        {
            toggle.value = value;
        }
    }

    public Texture Image
    {
        set
        {
            image.mainTexture = value;
        }
    }

    private void Start()
    {
        EventDelegate _event = new EventDelegate(OnChangeToggle);
        toggle.onChange.Add(_event);
    }

    void OnChangeToggle()
    {
        if(toggle.value)
            StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
    }
}
