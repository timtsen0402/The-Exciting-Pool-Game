using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
using static GameController;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static UnityEngine.Object[] BGM_s;
    public static AudioClip BGM_now;
    public static AudioSource BGM_source;

    private bool hasOut = true;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = false;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, x => x.name == name);
        if (s == null)
            return;
        s.source.Play();
    }
    void Start()
    {
        BGM_s = Resources.LoadAll("BGMs", typeof(AudioClip));
        BGM_source = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SettingManager.PartyTime();
        }
    }
    private void Update()
    {
        if(CueBall.transform.position.y < -100f && hasOut)
        {
            BGM_now = (AudioClip)BGM_s[0];
            BGM_source.clip = BGM_now;
            BGM_source.Play();
            hasOut = false;
        }
    }
}
