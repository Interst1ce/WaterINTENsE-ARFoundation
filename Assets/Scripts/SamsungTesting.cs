using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamsungTesting : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
    }
}
