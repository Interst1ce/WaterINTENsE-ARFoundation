using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioreactorHighlights : MonoBehaviour {
    public void DisableHighlights() {
        GameObject.Find("HighLights").SetActive(false);
    }

    public void DisableWaterEffects() {
        GameObject.Find("Bypass_Outlet").SetActive(false);
    }
}
