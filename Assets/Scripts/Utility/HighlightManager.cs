using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HighlightManager : MonoBehaviour {
    public Material glowMat;
    List<Material> ogMats = new List<Material>();
    Material highlightMat;
    //Material alphaHighlightMat;
    MeshRenderer objRenderer;
    //[HideInInspector]
    public bool glow = true;

    List<MeshRenderer> glowObjects = new List<MeshRenderer>();

    public void StartGlow(GameObject highlightObj) {
        if (highlightObj.GetComponent<MeshRenderer>() != null) {
            MatSwap(highlightObj.GetComponent<MeshRenderer>());
        } else {
            MeshRenderer[] renderers = highlightObj.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer rend in renderers) {
                if (rend != null) MatSwap(rend);
            }
        }
        glow = true;
        Glow();
    }

    public void StartGlow(List<GameObject> highlightObjs) {
        foreach (GameObject obj in highlightObjs) {
            if (obj.GetComponent<MeshRenderer>() != null) {
                MatSwap(obj.GetComponent<MeshRenderer>());
            } else {
                MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer rend in renderers) {
                    if(rend != null) MatSwap(rend);
                }
            }
        }
        glow = true;
        Glow();
    }

    public async void StartGlow(GameObject highlightObj,float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        if (highlightObj.GetComponent<MeshRenderer>() != null) {
            MatSwap(highlightObj.GetComponent<MeshRenderer>());
        } else {
            MeshRenderer[] renderers = highlightObj.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer rend in renderers) {
                if (rend != null) MatSwap(rend);
            }
        }
        glow = true;
        Glow();
    }

    public async void StartGlow(List<GameObject> highlightObjs,float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        foreach (GameObject obj in highlightObjs) {
            if (obj.GetComponent<MeshRenderer>() != null) {
                MatSwap(obj.GetComponent<MeshRenderer>());
            } else {
                MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer rend in renderers) {
                    if (rend != null) MatSwap(rend);
                }
            }
        }
        glow = true;
        Glow();
    }

    public void MatSwap(MeshRenderer objRenderer) {
        highlightMat = objRenderer.material;
        Debug.Log(highlightMat.name);
        if (highlightMat.shader != glowMat.shader) {
            ogMats.Add(highlightMat);
            Material ogMat = ogMats[ogMats.Count - 1];
            highlightMat = glowMat;
            highlightMat.SetTexture("Albedo",ogMat.mainTexture);
            highlightMat.SetColor("Albedo_Tint",ogMat.color);
            highlightMat.SetTexture("Normal",ogMat.GetTexture("_BumpMap"));
            highlightMat.SetTexture("Metallic",ogMat.GetTexture("_MetallicGlossMap"));
            highlightMat.SetTexture("Occlusion",ogMat.GetTexture("_OcclusionMap"));
            objRenderer.material = highlightMat;
            glowObjects.Add(objRenderer);
        }
    }

    async void Glow() {
        //float t = 0;
        do {
            //t = Mathf.PingPong(Time.time,1);
            //foreach(MeshRenderer objRenderer in glowObjects) objRenderer.material.SetFloat("HighlightIntensity",Mathf.Lerp(1,4,t));
            await Task.Yield();
        } while (glow);
        /*while(t > 0) {
            t -= Time.deltaTime;
            foreach(MeshRenderer objRenderer in glowObjects) objRenderer.material.SetFloat("HighlightIntensity",Mathf.Lerp(4,1,t));
            await Task.Yield();
        }*/
        for (int i = 0; i < glowObjects.Count; i++) glowObjects[i].material = ogMats[i];
        glowObjects.Clear();
        ogMats.Clear();
    }
}
