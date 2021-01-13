using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GlovesDisable : MonoBehaviour {
    public async void DisableGloves() {
        await Task.Delay(TimeSpan.FromSeconds(0.833f));
        foreach(MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
            renderer.enabled = false;
        }
    }
}
