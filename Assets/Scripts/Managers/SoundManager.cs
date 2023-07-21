using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundsType
{
    BUTTON,
    ERROR,
    GETSTAR
}


public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundSound;
    public AudioSource effectSound;

    [SerializeField]
    List<AudioClip> lstSounds;


    public void Initalized()
    {

        SetBackgroundSound(StaticManager.Backend.backendGameData.SoundData.BackgroundVolum);
        SetEffectSound(StaticManager.Backend.backendGameData.SoundData.EffectVolum);
        BackgroundMuteSound(StaticManager.Backend.backendGameData.SoundData.IsOnBackgroundVolum);
        EffectMuteSound(StaticManager.Backend.backendGameData.SoundData.IsOnEffectVolum);
    }


    public void SetBackgroundSound(float sound)
    {
        backgroundSound.volume = sound;
    }

    public void SetEffectSound(float sound)
    {
        effectSound.volume = sound;
    }

    public void BackgroundMuteSound(bool value)
    {
        backgroundSound.mute = !value;
    }

    public void EffectMuteSound(bool value)
    {
        effectSound.mute = !value;
    }

    public void PlaySounds(SoundsType type)
    {
        effectSound.clip = lstSounds[(int)type];
        effectSound.Play();
    }
}
