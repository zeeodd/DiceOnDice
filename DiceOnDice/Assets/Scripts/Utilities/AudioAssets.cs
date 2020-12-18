using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAssets : MonoBehaviour
{
    private static AudioAssets _i;

    public static AudioAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("Prefabs/AudioAssets")) as GameObject).GetComponent<AudioAssets>();
            return _i;
        }
    }

    [System.Serializable]
    public struct AudioItem
    {
        public AudioManager.AudioType audioType;
        public AudioClip[] clips;
    }

    [Header("Sounds")]
    public AudioItem[] soundClips;

    [Header("Music")]
    public AudioItem[] musicClips;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
