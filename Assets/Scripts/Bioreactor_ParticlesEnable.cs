using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Bioreactor_ParticlesEnable : MonoBehaviour {
    public GameObject particlesToEnable;
    public async void EnableParticles(float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        particlesToEnable.SetActive(true);
    }

    public async void DisableParticles(float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        particlesToEnable.GetComponent<ParticleSystem>().Stop();
    }
}
