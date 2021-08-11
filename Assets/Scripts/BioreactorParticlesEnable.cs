using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BioreactorParticlesEnable : MonoBehaviour {
    public GameObject particlesToEnable;
    public void EnableParticles(float delay) {
        Enable(delay);
    }

    public void EnableParticles(int delay) {
        Enable(delay);
    }

    private async void Enable(float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        particlesToEnable.GetComponent<ParticleSystem>().Play();
    }

    public void DisableParticles(float delay) {
        Disable(delay);
    }

    public void DisableParticles(int delay) {
        Disable(delay);
    }

    private async void Disable(float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        particlesToEnable.GetComponent<ParticleSystem>().Stop();
    }
}
