using UnityEngine;

[System.Serializable]
public class SoundXinHonSaoundCu
{
    public string name;
    public TypeAudio type;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public enum TypeAudio
{
    Music,
    Sound
}
