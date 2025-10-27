using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public SoundXinHonSaoundCu[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (SoundXinHonSaoundCu s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume ;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        this.Play("Home");
    }

    public void Play(string name)
    {
        SoundXinHonSaoundCu s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void Stop(string name)
    {
        SoundXinHonSaoundCu s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public void VolumBySlider(float sliderSound, float sliderMusic)
    {
        foreach (SoundXinHonSaoundCu s in sounds)
        {
            if (s.type.ToString() == "Music")
            {
                s.source.volume = s.volume * sliderMusic;
            }
            else if (s.type.ToString() == "Sound")
            {
                s.source.volume = s.volume * sliderSound;
            }
        }
    }
    public void MuteAdio(bool val, string _string)
    {

        foreach (SoundXinHonSaoundCu s in sounds)
        {
            if (s.type.ToString() == _string)
            {
                s.source.mute = val;
            }
        }
    }
}