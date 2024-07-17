using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Script containing all sounds used in the game.
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// List of sounds which can be used in the game.
    /// </summary>
    [SerializeField]
    [UnityEngine.Tooltip("List of sounds which can be used in the game.")]
    public List<Sound> Sounds;
    /// <summary>
    /// Current static instance of the class.
    /// </summary>
    public static AudioManager instance;
    
    private static readonly Object syncRoot = new Object();
    
    /// <summary>
    /// Current static instance of the class.
    /// </summary>
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
                        if (instance == null)
                            Debug.LogError("SingletoneBase<T>: Could not found GameObject of type " + nameof(AudioManager));
                    }
                }
            }
            return instance;
        }
        set { }
    }
    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            for (int i = 0; i < Sounds.Count; i++)
            {
                Sounds[i].Source = gameObject.AddComponent<AudioSource>();
                Sounds[i].Source.clip = Sounds[i].Clip;
                Sounds[i].Source.volume = Sounds[i].Volume;
                Sounds[i].Source.pitch = Sounds[i].Pitch;
                Sounds[i].Source.loop = Sounds[i].Loop;
            }
        }
        
    }

    
    /// <summary>
    /// Play given sound.
    /// </summary>
    /// <param name="soundObj">Sound object to be played.</param>
    public void Play(Sound soundObj)
    {
        Sound s = Array.Find(Sounds.ToArray(), sound => sound.Name == soundObj.Name);
        if (s == null)
            return;
        s.Source.Play();
    }
    
    /// <summary>
    /// Stop given sound.
    /// </summary>
    /// <param name="soundObj">Sound object to be played.</param>
    public void Stop(Sound soundObj)
    {
        Sound s = Array.Find(Sounds.ToArray(), sound => sound.Name == soundObj.Name);
        if (s == null)
            return;
        s.Source.Stop();
    }
    
    /// <summary>
    /// Set pitch of the given sound.
    /// </summary>
    /// <param name="soundObj">Sound object.</param>
    /// <param name="pitch">Pitch value.</param>
    public void SetPitch(Sound soundObj, float pitch)
    {
        Sound s = Array.Find(Sounds.ToArray(), sound => sound.Name == soundObj.Name);
        if (s == null)
            return;
        s.Source.pitch = pitch;
    }
    /// <summary>
    /// Check if the given sound is currently playing.
    /// </summary>
    /// <param name="soundObj">Sound object.</param>
    public bool IsPlaying(Sound soundObj)
    {
        Sound s = Array.Find(Sounds.ToArray(), sound => sound.Name == soundObj.Name);
        if (s == null)
            return false;
        return s.Source.isPlaying;
    }

    /// <summary>
    /// Pause or unpause all sounds.
    /// </summary>
    /// <param name="isPause">True if pause the sound. False if unpause the sound.</param>
    public void PauseSounds(bool isPause)
    {
        if (isPause)
        {
            foreach (Sound s in Sounds)
            {
                s.Source.Pause();
            }
        }
        else
        {
            foreach (Sound s in Sounds)
            {
                s.Source.UnPause();
            }
        }
    }
    
}

