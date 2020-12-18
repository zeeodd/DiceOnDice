using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class AudioManager
{
    #region AudioManager Variables
    public enum AudioType
    {
        Sample
    }

    private Dictionary<AudioType, float> audioDelays;
    private GameObject oneShotGameObject;
    private AudioSource oneShotAudioSource;
    private GameObject musicGameObject;
    private AudioSource musicAudioSource;
    private AudioType currentMusicType = AudioType.Sample;
    #endregion

    #region AudioType Delays
    public float sampleDelay = 0.1f;
    #endregion

    #region Class Functions
    public void Initialize()
    {
        audioDelays = new Dictionary<AudioType, float>();
        InitializeAudioDelays();
    }

    private void InitializeAudioDelays()
    {
        audioDelays[AudioType.Sample] = 0f;
    }
    #endregion

    #region Audio Functions
    public void Reset()
    {
        if (musicAudioSource != null) Object.Destroy(musicAudioSource);
        if (oneShotAudioSource != null) Object.Destroy(oneShotAudioSource);
    }

    /*
     *  Plays an audio clip in world space and deletes the game object after it has finished
     *  EX: Services.AudioManager.PlayWorldSound( AudioManager.AudioType.Sample, new Vector3(0f, 0f, 0f) );
     */
    public void PlayWorldSound(AudioType audioType, Vector3 pos, float vol = 0.5f, bool pickRandomSound = false)
    {
        if (Globals.muted) vol = 0;

        if (CanPlaySound(audioType))
        {
            GameObject soundGameObject = new GameObject("WorldSound");
            soundGameObject.transform.position = pos;
            soundGameObject.transform.SetParent(AudioAssets.i.transform);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            if (pickRandomSound) audioSource.clip = GetRandomAudioClip(audioType);
            else audioSource.clip = GetAudioClip(audioType);
            audioSource.volume = vol;
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    /*
     *  Plays an audio clip
     *  EX: Services.AudioManager.PlaySound( AudioManager.AudioType.Sample );
     */
    public void PlaySound(AudioType audioType, float vol = 0.5f, bool pickRandomSound = false)
    {
        if (Globals.muted) vol = 0;

        if (CanPlaySound(audioType))
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotGameObject.transform.SetParent(AudioAssets.i.transform);
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
                oneShotAudioSource.volume = vol;

                if (pickRandomSound) oneShotAudioSource.PlayOneShot(GetRandomAudioClip(audioType));
                else oneShotAudioSource.PlayOneShot(GetAudioClip(audioType), vol);
            }
            else
            {
                var newoneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
                newoneShotAudioSource.volume = vol;

                if (pickRandomSound) newoneShotAudioSource.PlayOneShot(GetRandomAudioClip(audioType));
                else newoneShotAudioSource.PlayOneShot(GetAudioClip(audioType), vol);
            }
        }
    }

    public void FadeOutSound(AudioType audioType, float finalVolume, float duration)
    {
        var audioSources = oneShotGameObject.GetComponents(typeof(AudioSource));

        foreach (AudioSource asource in audioSources)
        {
            if (asource.isPlaying && asource.volume > 0f && CheckSoundType(audioType))
            {
                asource.DOFade(finalVolume, duration);
            }
        }
    }

    public void FadeOutMusic(AudioType audioType, float finalVolume, float duration)
    {
        var audioSources = musicGameObject.GetComponents(typeof(AudioSource));

        foreach (AudioSource asource in audioSources)
        {
            if (asource.isPlaying && asource.volume > 0f && CheckMusicType(audioType))
            {
                asource.DOFade(finalVolume, duration);
            }
        }
    }

    /*
     *  Plays a music clip
     *  EX: Services.AudioManager.PlayMusic( AudioManager.AudioType.Sample );
     */
    public void PlayMusic(AudioType audioType, float vol = 0.5f)
    {
        if (Globals.muted) vol = 0;

        if (CanPlaySound(audioType))
        {
            if (musicGameObject == null)
            {
                musicGameObject = new GameObject("Music");
                musicGameObject.transform.SetParent(AudioAssets.i.transform);
                musicAudioSource = musicGameObject.AddComponent<AudioSource>();
                musicAudioSource.volume = vol;
                musicAudioSource.clip = GetMusicClip(audioType);
                musicAudioSource.loop = true;
                musicAudioSource.Play();
            }
            else
            {
                var newMusicAudioSource = musicGameObject.AddComponent<AudioSource>();
                newMusicAudioSource.volume = vol;
                newMusicAudioSource.clip = GetMusicClip(audioType);
                newMusicAudioSource.loop = true;
                newMusicAudioSource.Play();
            }
        }
    }

    /*
     *  Checks when a clip was last played and decides if it can be played again
     */
    private bool CanPlaySound(AudioType audioType)
    {
        switch (audioType)
        {
            #region Sample Code for Delaying a Sound
            /*
             *  Below is an example case where we can delay the playing of a sound so
             *  it doesn't play every single frame
             */
            case AudioType.Sample:
                if (audioDelays.ContainsKey(audioType))
                {
                    float timeLastPlayed = audioDelays[audioType];
                    float timeMax = sampleDelay;
                    if (timeLastPlayed + timeMax < Time.time)
                    {
                        audioDelays[audioType] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            #endregion
            default:
                return true;
        }
    }

    /*
     *  Grabs a single AudioClip in the Sounds array  based on the passed-in AudioType enum
     */
    private AudioClip GetAudioClip(AudioType audioType)
    {
        foreach (AudioAssets.AudioItem audioItem in AudioAssets.i.soundClips)
        {
            if (audioItem.audioType == audioType && audioItem.clips.Length == 1)
            {
                return audioItem.clips[0];
            }
        }
        Debug.LogError("Sound " + audioType + " not found!");
        return null;
    }

    /*
     *  Grabs a random AudioClip based on the passed-in AudioType enum
     */
    private AudioClip GetRandomAudioClip(AudioType audioType)
    {
        foreach (AudioAssets.AudioItem audioItem in AudioAssets.i.soundClips)
        {
            if (audioItem.audioType == audioType && audioItem.clips.Length > 1)
            {
                return audioItem.clips[Random.Range(0, audioItem.clips.Length)];
            }
        }
        Debug.LogError("Sound " + audioType + " not found!");
        return null;
    }

    /*
     *  Grabs a single AudioClip in the Music array based on the passed-in AudioType enum
     */
    private AudioClip GetMusicClip(AudioType audioType)
    {
        foreach (AudioAssets.AudioItem audioItem in AudioAssets.i.musicClips)
        {
            if (audioItem.audioType == audioType && audioItem.clips.Length == 1)
            {
                return audioItem.clips[0];
            }
        }
        Debug.LogError("Sound " + audioType + " not found!");
        return null;
    }

    /*
     *  Checks the AudioType of a given AudioClip
     */
    private bool CheckMusicType(AudioType audioType)
    {
        bool returnBool = false;

        foreach (AudioAssets.AudioItem audioItem in AudioAssets.i.musicClips)
        {
            if (audioItem.clips.Length > 1 && audioItem.audioType == audioType)
            {
                returnBool = true;
            }
        }

        return returnBool;
    }

    private bool CheckSoundType(AudioType audioType)
    {
        bool returnBool = false;

        foreach (AudioAssets.AudioItem audioItem in AudioAssets.i.soundClips)
        {
            if (audioItem.clips.Length > 0 && audioItem.audioType == audioType)
            {
                returnBool = true;
            }
        }

        return returnBool;
    }
    #endregion
}