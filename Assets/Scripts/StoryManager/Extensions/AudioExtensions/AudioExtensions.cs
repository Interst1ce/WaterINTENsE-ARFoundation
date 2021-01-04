using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AudioExtensions : MonoBehaviour {
    public AudioSource source;

    public void PlaySFX(AudioClip sfx) {
        source.PlayOneShot(sfx);
    }

    public async void PlaySFXDelayed(AudioClip sfx, float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        source.PlayOneShot(sfx);
    }
}
