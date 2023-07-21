using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option_Popup : MonoBehaviour
{
    [SerializeField]
    UILabel totalWalk_Label;

    [SerializeField]
    UISlider effect_slider;

    [SerializeField]
    UISlider background_slider;

    [SerializeField]
    UIToggle effect_Toggle;

    [SerializeField]
    UIToggle background_Toggle;

    [SerializeField]
    UIButton close_btn;


    public float Effect
    {
        set
        {
            effect_slider.value = value;
        }
    }

    public float Background
    {
        set
        {
            background_slider.value = value;
        }
    }

    public bool Effect_Toggle
    {
        set
        {
            effect_Toggle.value = value;
        }
    }

    public bool Background_Toggle
    {
        set
        {
            background_Toggle.value = value;
        }
    }

    public string TotalWalk
    {
        set
        {
            totalWalk_Label.text = value;
        }
    }

    private void OnEnable()
    {
        /*background_slider.value = StaticManager.Backend.backendGameData.SoundData.BackgroundVolum;
        effect_slider.value = StaticManager.Backend.backendGameData.SoundData.EffectVolum;
        effect_Toggle.value = StaticManager.Backend.backendGameData.SoundData.IsOnEffectVolum;
        background_Toggle.value = StaticManager.Backend.backendGameData.SoundData.IsOnBackgroundVolum;
        NGUITools.BringForward(gameObject);*/
    }

    private void Start()
    {
        AddLisener();
        background_slider.value = StaticManager.Backend.backendGameData.SoundData.BackgroundVolum;
        effect_slider.value = StaticManager.Backend.backendGameData.SoundData.EffectVolum;
        effect_Toggle.value = StaticManager.Backend.backendGameData.SoundData.IsOnEffectVolum;
        background_Toggle.value = StaticManager.Backend.backendGameData.SoundData.IsOnBackgroundVolum;
        NGUITools.BringForward(gameObject);
    }

    void AddLisener()
    {
        EventDelegate _event = new EventDelegate(OnClickClose_Btn);
        close_btn.onClick.Add(_event);

        _event = new EventDelegate(OnChangeBackgroundSound);
        background_slider.onChange.Add(_event);

        _event = new EventDelegate(OnChangeEffectSound);
        effect_slider.onChange.Add(_event);

        _event = new EventDelegate(OnChangeBackgroundSound_Toggle);
        background_Toggle.onChange.Add(_event);

        _event = new EventDelegate(OnChangeEffectSound_Toggle);
        effect_Toggle.onChange.Add(_event);
    }

    void OnChangeBackgroundSound_Toggle()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        StaticManager.Sound.BackgroundMuteSound(background_Toggle.value);
        StaticManager.Backend.backendGameData.SoundData.IsOnBackgroundVolum = !background_Toggle.value;
    }

    void OnChangeEffectSound_Toggle()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        StaticManager.Sound.EffectMuteSound(effect_Toggle.value);
        StaticManager.Backend.backendGameData.SoundData.IsOnEffectVolum = !effect_Toggle.value;
    }


    void OnChangeBackgroundSound()
    {
        StaticManager.Backend.backendGameData.SoundData.SetBackgroundVolum(background_slider.value);
        StaticManager.Sound.SetBackgroundSound(background_slider.value);
        
    }

    void OnChangeEffectSound()
    {
        StaticManager.Backend.backendGameData.SoundData.SetEffectVolum(effect_slider.value);
        StaticManager.Sound.SetEffectSound( effect_slider.value);
    }

    void OnClickClose_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        StaticManager.Backend.backendGameData.SoundData.Update(callback =>
        {
            if (callback.IsSuccess())
            {
                gameObject.SetActive(false);
            }
        });
        
    }

    
}
