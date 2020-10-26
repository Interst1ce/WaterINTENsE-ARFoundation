using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StirrerReadout : MonoBehaviour {
    public TextMeshPro readout;

    string text = "";
    int counter = 0;
    int targetCount = 0;

    public void Count(int target) {
        targetCount = target;
        if (counter < targetCount) {
            StartCoroutine("CountUp");
        } else StartCoroutine("CountDown");
    }

    IEnumerator CountUp() {
        while (counter <= targetCount) {
            text = "00" + counter;
            counter++;
            readout.text = text;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    IEnumerator CountDown() {
        while (counter >= targetCount) {
            text = "00" + counter;
            counter--;
            readout.text = text;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
