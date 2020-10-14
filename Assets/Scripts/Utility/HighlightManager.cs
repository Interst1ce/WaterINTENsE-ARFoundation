using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HighlightManager : MonoBehaviour {
    [Range(0.1f,5)]
    public float outlineThicknessDefault = 1;
    [HideInInspector]
    public bool glow = false;

    List<Outline> glowObjects = new List<Outline>();

    public void StartGlow(GameObject highlightObj) {
        Outline outline = highlightObj.GetComponent<Outline>();
        if (outline != null) {
            outline.enabled = true;
            glowObjects.Add(outline);
        } else {
            outline = highlightObj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow * 2;
            outline.OutlineWidth = outlineThicknessDefault;
            outline.enabled = true;
            glowObjects.Add(outline);
        }
        glow = true;
        Glow();
    }

    public void StartGlow(List<GameObject> highlightObjs) {
        foreach (GameObject obj in highlightObjs) {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null) {
                outline.enabled = true;
                glowObjects.Add(outline);
            } else {
                outline = obj.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = outlineThicknessDefault;
                outline.enabled = true;
                glowObjects.Add(outline);
            }
        }
        glow = true;
        Glow();
    }

    public async void StartGlow(GameObject highlightObj,float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        Outline outline = highlightObj.GetComponent<Outline>();
        if (outline != null) {
            outline.enabled = true;
            glowObjects.Add(outline);
        } else {
            outline = highlightObj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = outlineThicknessDefault;
            outline.enabled = true;
            glowObjects.Add(outline);
        }
        glow = true;
        Glow();
    }

    public async void StartGlow(List<GameObject> highlightObjs,float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        foreach (GameObject obj in highlightObjs) {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null) {
                outline.enabled = true;
                glowObjects.Add(outline);
            } else {
                outline = obj.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = outlineThicknessDefault;
                outline.enabled = true;
                glowObjects.Add(outline);
            }
        }
        glow = true;
        Glow();
    }

    async void Glow() {
        do {
            await Task.Yield();
        } while (glow);
        foreach (Outline outline in glowObjects) outline.enabled = false;
    }
    /*
    public void StartGlow(GameObject highlightObj) {
        if (highlightObj.GetComponent<MeshRenderer>() != null) {
            MatSwap(highlightObj.GetComponent<MeshRenderer>());
        } else if (highlightObj.GetComponent<SkinnedMeshRenderer>() != null) {
            MatSwap(highlightObj.GetComponent<SkinnedMeshRenderer>());
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
            } else if (obj.GetComponent<SkinnedMeshRenderer>() != null) {
                MatSwap(obj.GetComponent<SkinnedMeshRenderer>());
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

    public async void StartGlow(GameObject highlightObj,float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        if (highlightObj.GetComponent<MeshRenderer>() != null) {
            MatSwap(highlightObj.GetComponent<MeshRenderer>());
        } else if (highlightObj.GetComponent<SkinnedMeshRenderer>() != null) {
            MatSwap(highlightObj.GetComponent<SkinnedMeshRenderer>());
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
            } else if (obj.GetComponent<SkinnedMeshRenderer>() != null) {
                MatSwap(obj.GetComponent<SkinnedMeshRenderer>());
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

    public void MatSwap(SkinnedMeshRenderer objRenderer) {
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
            glowSkinnedObjects.Add(objRenderer);
        }
    }

    async void Glow() {
        do {
            await Task.Yield();
        } while (glow);
        for (int i = 0; i < glowObjects.Count; i++) glowObjects[i].material = ogMats[i];
        for (int i = 0; i < glowSkinnedObjects.Count; i++) glowSkinnedObjects[i].material = ogMats[i];
        glowObjects.Clear();
        glowSkinnedObjects.Clear();
        ogMats.Clear();
    }*/
}
