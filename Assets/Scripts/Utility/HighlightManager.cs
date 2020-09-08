using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HighlightManager : MonoBehaviour {
    public Material glowMat;
    List<Material> ogMats = new List<Material>();
    Material highlightMat;
    MeshRenderer objRenderer;
    //[HideInInspector]
    public bool glow = true;

    List<MeshRenderer> glowObjects = new List<MeshRenderer>();

    public void StartGlow(GameObject highlightObj) {
        MatSwap(highlightObj.GetComponent<MeshRenderer>());
        glow = true;
        Glow();
    }

    public void StartGlow(List<GameObject> highlightObjs) {
        foreach (GameObject obj in highlightObjs) MatSwap(obj.GetComponent<MeshRenderer>());
        glow = true;
        Glow();
    }

    public async void StartGlow(GameObject highlightObj, float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        MatSwap(highlightObj.GetComponent<MeshRenderer>());
        glow = true;
        Glow();
    }

    public async void StartGlow(List<GameObject> highlightObjs, float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        foreach (GameObject obj in highlightObjs) MatSwap(obj.GetComponent<MeshRenderer>());
        glow = true;
        Glow();
    }

    public void MatSwap(MeshRenderer objRenderer) {
        highlightMat = objRenderer.material;
        if (highlightMat.shader != glowMat.shader) {
            ogMats.Add(highlightMat);
            Material ogMat = ogMats[ogMats.Count - 1];
            highlightMat = glowMat;
            highlightMat.SetTexture("Albedo",ogMat.mainTexture);
            highlightMat.SetTexture("Normal",ogMat.GetTexture("_BumpMap"));
            highlightMat.SetTexture("Metallic",ogMat.GetTexture("_MetallicGlossMap"));
            highlightMat.SetTexture("Occlusion",ogMat.GetTexture("_OcclusionMap"));
            objRenderer.material = highlightMat;
            glowObjects.Add(objRenderer);
        }
    }

    async void Glow() {
        float t = 0;
        do {
            t = Mathf.PingPong(Time.time,1);
            foreach(MeshRenderer objRenderer in glowObjects) objRenderer.material.SetFloat("HighlightIntensity",Mathf.Lerp(1,4,t));
            await Task.Yield();
        } while (glow);
        while(t > 0) {
            t -= Time.deltaTime;
            foreach(MeshRenderer objRenderer in glowObjects) objRenderer.material.SetFloat("HighlightIntensity",Mathf.Lerp(4,1,t));
            await Task.Yield();
        }
        for (int i = 0; i < glowObjects.Count; i++) glowObjects[i].material = ogMats[i];
        glowObjects.Clear();
        ogMats.Clear();
    }
}
