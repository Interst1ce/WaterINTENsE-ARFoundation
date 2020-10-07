using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShadowCastToggle : MonoBehaviour {
    [SerializeField]
    GameObject sludgeJudge;

    public void ShadowToggle() {
        foreach(MeshRenderer mesh in sludgeJudge.GetComponentsInChildren<MeshRenderer>()) {
            if (mesh.shadowCastingMode == 0) {
                mesh.shadowCastingMode = ShadowCastingMode.On;
            } else mesh.shadowCastingMode = 0;
        }
    }
}
