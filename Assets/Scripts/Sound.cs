using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Sound {

    public string name;
    public AudioClip clip;
    public float volume;
    public float pitch;

    [HideInInspector] public AudioSource source;
}