using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager instance = null;

    public List<Sound> soundList;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        else {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound sound in soundList) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }

    public void PlaySound(string soundName) {
        foreach (Sound sound in soundList) {
            if (sound.name == soundName) {
                sound.source.Play();
            }
        }
    }
}
