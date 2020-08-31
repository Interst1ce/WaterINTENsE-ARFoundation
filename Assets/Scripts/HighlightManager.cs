using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HighlightManager : MonoBehaviour {
    public Material glowMat;
    Material ogMat;
    Material highlightMat;
    MeshRenderer objRenderer;
    [HideInInspector]
    public bool glow = true;

    public void StartGlow(GameObject highlightObj) {
        MatSwap(highlightObj.GetComponent<MeshRenderer>());
        Glow(highlightObj);
    }

    public async void StartGlow(GameObject highlightObj, float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        MatSwap(highlightObj.GetComponent<MeshRenderer>());
        Glow(highlightObj);
    }

    public void MatSwap(MeshRenderer objRenderer) {
        highlightMat = objRenderer.material;
        if (highlightMat.shader != glowMat.shader) {
            ogMat = highlightMat;
            highlightMat = glowMat;
            highlightMat.SetTexture("Albedo",ogMat.mainTexture);
            highlightMat.SetTexture("Normal",ogMat.GetTexture("_BumpMap"));
            highlightMat.SetTexture("Metallic",ogMat.GetTexture("_MetallicGlossMap"));
            highlightMat.SetTexture("Occlusion",ogMat.GetTexture("_OcclusionMap"));
            objRenderer.material = highlightMat;
        }
    }

    async void Glow(GameObject highlightObj) {
        objRenderer = highlightObj.GetComponent<MeshRenderer>();
        float t = 0;
        do {
            t = Mathf.PingPong(Time.time,1);
            objRenderer.material.SetFloat("HighlightIntensity",Mathf.Lerp(1,4,t));
            await Task.Yield();
        } while (glow);
        while(t > 0) {
            t -= Time.deltaTime;
            objRenderer.material.SetFloat("HighlightIntensity",Mathf.Lerp(4,1,t));
            await Task.Yield();
        }
        objRenderer.material = ogMat;
    }
    /*
    IEnumerator Glow(GameObject highlightObj) {
        Debug.Log("Glow coroutine started");
        objRenderer = highlightObj.GetComponent<MeshRenderer>();
        float t = 0;
        //yield return new WaitForSecondsRealtime(1f);
        do {
            t = Mathf.PingPong(Time.time,1);
            objRenderer.material.SetFloat("HighlightIntensity",Mathf.Lerp(1,4,t));
            yield return null;
        } while (glow);
        while(t > 0) {
            t -= Time.deltaTime;
            objRenderer.material.SetFloat("HighlightIntensity",Mathf.Lerp(4,1,t));
            yield return null;
        }
        objRenderer.material = ogMat;
    }*/
}
