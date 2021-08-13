using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GlovesDisable : MonoBehaviour {

    [SerializeField]
    GameObject gloves;



    public async void DisableGloves() {
        gloves.GetComponent<Collider>().enabled = false;
        await Task.Delay(TimeSpan.FromSeconds(0.833f));
        foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
            renderer.enabled = false;
        }
    }
}
