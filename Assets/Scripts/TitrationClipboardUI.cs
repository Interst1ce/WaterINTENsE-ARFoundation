using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitrationClipboardUI : MonoBehaviour {
    public TextMeshProUGUI last, first, free;
    public void FadeUIIn() {
        StartCoroutine(UIAlphaFade(last,6));
        StartCoroutine(UIAlphaFade(first,9));
        StartCoroutine(UIAlphaFade(free,11));
    }

    IEnumerator UIAlphaFade(TextMeshProUGUI text, float delay = 0) {
        float t = 0;
        while (t < delay) {
            t += Time.deltaTime;
            yield return null;
        }
        t = 0; 
        Color32 newColor;
        while (t < 1) {
            t += Time.deltaTime;
            newColor = new Color32(0,0,0,(byte)((text.color.a + t) * 255));
            text.color = newColor;
            yield return null;
        }
        newColor = new Color32(0,0,0,255);
        text.color = newColor;
    }
}
