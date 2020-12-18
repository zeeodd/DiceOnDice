using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) ToggleAudio();
        if (Input.GetKeyDown(KeyCode.DownArrow)) DecreaseVolume();
        if (Input.GetKeyDown(KeyCode.UpArrow)) IncreaseVolume();
    }

    public void IncreaseVolume()
    {
        AudioListener.volume += 0.1f;
    }

    public void DecreaseVolume()
    {
        AudioListener.volume -= 0.1f;
    }

    public void DisableAudio()
    {
        SetAudioMute(false);
    }

    public void EnableAudio()
    {
        SetAudioMute(true);
    }

    public void ToggleAudio()
    {
        if (Globals.muted)
            DisableAudio();
        else
            EnableAudio();
    }

    private void SetAudioMute(bool mute)
    {
        AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        for (int index = 0; index < sources.Length; ++index)
        {
            sources[index].mute = mute;
        }
        Globals.muted = mute;
    }
}