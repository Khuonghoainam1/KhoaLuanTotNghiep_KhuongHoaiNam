using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yurowm.GameCore;
using System;

public class AudioAssistant : MonoBehaviourAssistant<AudioAssistant>
{
    public float musicVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("Music Volume"))
                return 1f;
            return PlayerPrefs.GetFloat("Music Volume");
        }
        set
        {
            PlayerPrefs.SetFloat("Music Volume", value);
            if (musicSource)
                musicSource.volume = value;
        }
    }

    public float soundVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("SFX Volume"))
                return 1f;
            return PlayerPrefs.GetFloat("SFX Volume");
        }
        set
        {   
            PlayerPrefs.SetFloat("SFX Volume", value);
            foreach (AudioSource source in soundSources)
            {
                source.volume = value;
            }
        }
    }

    AudioSource musicSource;
    List<AudioSource> soundSources = new List<AudioSource>();
    
    public List<MusicTrack> musics = new List<MusicTrack>();
    public List<Sound> sounds = new List<Sound>();
    static List<string> mixBuffer = new List<string>();
    static float mixBufferClearDelay = 0.1f;

    public void Init()
    {
        CreateAudioSource();

        UpdatePath();
    }

    void CreateAudioSource()
    {
        // Create Music Source
        if (musicSource == null)
        {
            GameObject objMusicSource = new GameObject("MusicSource");
            objMusicSource.transform.parent = transform;
            musicSource = objMusicSource.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }

        // Create Sound Source List
        if (soundSources.Count == 0)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject obj = new GameObject("SoundSource" + (i + 1));
                obj.transform.parent = transform;
                AudioSource src = obj.AddComponent<AudioSource>();
                src.playOnAwake = false;
                src.loop = false;

                soundSources.Add(src);
            }
        }

        StartCoroutine(MixBufferRoutine());
        musicVolume = musicVolume;
        soundVolume = soundVolume;
    }

    // Coroutine responsible for limiting the frequency of playing sounds
    IEnumerator MixBufferRoutine()
    {
        float time = 0;

        while (true)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
            if (time >= mixBufferClearDelay)
            {
                mixBuffer.Clear();
                time = 0;
            }
        }
    }

    private int GetUnusedAudioSourceIdx(AudioClip clipSame = null)
    {
        for (int i = 0; i < soundSources.Count; i++)
        {
            if (!soundSources[i].isPlaying)
            {
                return i;
            }
        }
        return 0;   //if everything is used up, use item number zero
    }

    public void UpdatePath()
    {
        sounds.ForEach(x => x.fullName = (x.path.IsNullOrEmpty() ? "" : x.path + "/") + x.name);
        musics.ForEach(x => x.fullName = (x.path.IsNullOrEmpty() ? "" : x.path + "/") + x.name);
    }

    // Launching a music track
    string currentMusic;
    public float PlayMusic(string musicName)
    {
        if (musicName == currentMusic)
        {
            return 0;
        }

        currentMusic = musicName;
        MusicTrack music = GetMusicByName(musicName);
        if (music != null)
        {
            StartCoroutine(CrossFade(music.track));
            return music.track.length;
        }

        return 0;
    }

    MusicTrack GetMusicByName(string name)
    {
        return musics.Find(x => x.name == name);
    }

    // A smooth transition from one to another music
    IEnumerator CrossFade(AudioClip to)
    {
        if (musicSource == null) yield break;
        float delay = 0.1f;
        if (musicSource.clip != null)
        {
            while (delay > 0)
            {
                musicSource.volume = delay * musicVolume;
                delay -= Time.unscaledDeltaTime;
                yield return null;
            }
        }
        musicSource.clip = to;
        if (to == null)
        {
            StopMusic();
            yield break;
        }
        delay = 0;
        if (!musicSource.isPlaying) musicSource.Play();
        while (delay < 0.3f)
        {
            musicSource.volume = delay * musicVolume;
            delay += Time.unscaledDeltaTime;
            yield return null;
        }
        musicSource.volume = musicVolume;
    }

    public void StopMusic()
    {
        if (musicSource)
            musicSource.Stop();

        currentMusic = null;
    }

    // A single sound effect
    public static float PlaySound(string clip)
    {
        Sound sound = main.GetSoundByName(clip);

        if (sound != null && sound.clips.Count > 0 && !mixBuffer.Contains(clip))
        {
            mixBuffer.Add(clip);
            AudioClip audioClip = sound.clips.GetRandom();
            main.soundSources[main.GetUnusedAudioSourceIdx()].PlayOneShot(audioClip);
            return audioClip.length;
        }

        return 0;
    }

    public static void PlaySoundWhile(string clip, Func<bool> whileCondition)
    {
        Sound sound = main.GetSoundByName(clip);

        if (sound != null && sound.clips.Count > 0 && !mixBuffer.Contains(clip))
        {
            mixBuffer.Add(clip);
            AudioClip audioClip = sound.clips.GetRandom();            
            main.StartCoroutine(main.IE_PlaySoundWhile(main.soundSources[main.GetUnusedAudioSourceIdx()], audioClip, whileCondition));
        }
    }

    public static void PlaySoundWhile(AudioClip clip, Func<bool> whileCondition)
    {
        main.StartCoroutine(main.IE_PlaySoundWhile(main.soundSources[main.GetUnusedAudioSourceIdx()], clip, whileCondition));
    }

    IEnumerator IE_PlaySoundWhile(AudioSource source, AudioClip clip, Func<bool> whileCondition)
    {
        source.loop = true;
        source.clip = clip;
        source.Play();

        while(whileCondition() == true)
        {
            yield return null;
        }

        source.Stop();
        source.clip = null;
        source.loop = false;
    }

    Sound GetSoundByName(string fullName)
    {
        return sounds.Find(x => x.fullName == fullName);
    }

    public static float PlaySound(AudioClip clip, float delay = 0)
    {
        if (clip == null)
        {
            return 0;
        }

        main.StartCoroutine(IE_PlaySound(clip, delay));

        return clip.length;
    }

    static IEnumerator IE_PlaySound(AudioClip clip, float delay)
    {
        yield return WaitForSecondsCache.Get(delay);

        main.soundSources[main.GetUnusedAudioSourceIdx()].PlayOneShot(clip);
    }

    public void StopSound()
    {
        soundSources.ForEach(x => x.Stop());
    }

    // sound delay
    public void ShotDelay(string clip, float delay = 0)
    {
        StartCoroutine(IEShotDelay(clip, delay));
    }

    IEnumerator IEShotDelay(string clip, float delay)
    {
        yield return WaitForSecondsCache.Get(delay);
        PlaySound(clip);
    }

    public void ChangeMusicVolume(float v)
    {
        musicSource.volume = v;
    }

    public void ChangeSoundVolume(float v)
    {
        foreach (AudioSource source in soundSources)
        {
            source.volume = v;
        }
    }

    [System.Serializable]
    public class MusicTrack
    {
        public string name;
        public string path;
        public string fullName;
        public AudioClip track;
        public int id = 0;
        [Range(0f, 1f)]
        public float volumMussic;
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public string path;
        public string fullName;
        public List<AudioClip> clips = new List<AudioClip>();
        public int id = 0;
    }
}
