using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class CoCUIFadeIn : MonoBehaviour {
    public float firstFadeDelay = 0;
    public List<GameObject> panels = new List<GameObject>();

    public async void CoCFade() {
        await Task.Delay(TimeSpan.FromSeconds(firstFadeDelay));
        foreach(GameObject panel in panels) {
            await Task.Delay(TimeSpan.FromSeconds(1));
            TextMeshProUGUI[] texts = panel.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts) {
                Fade(text);
            }
        }
    }

    async void Fade(TextMeshProUGUI text) {
        //iterate through child objects and lerp each textmeshprougui vertex color alpha from 0-1
        float t = 0;
        Color original = text.color;
        Color fadeTo = new Color(original.r,original.g,original.b,100);
        while(t < 1) {
            t += Time.deltaTime;
            Color.Lerp(original,fadeTo,t);
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
    }
}
