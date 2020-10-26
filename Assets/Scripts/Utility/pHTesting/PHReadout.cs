using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PHReadout : MonoBehaviour {
    public TextMeshPro readout;

    bool alt = false;

    public void AlternateTrigger() {
        if (alt) {
            pH4Update();
        } else pH7Update();
        alt = !alt;
    }

    public void pH4Update() {
        readout.text = "4.0";
    }

    public void pH7Update() {
        readout.text = "7.0";
    }

    public void SampleUpdate() {
        readout.text = "6.2";
    }

    public void naUpdate() {
        readout.text = "N/A";
    }
}
