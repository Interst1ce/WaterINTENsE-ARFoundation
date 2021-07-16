using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeScene : MonoBehaviour {

    [SerializeField]
    GameObject scene;
    float oldHeight;
    Vector3 oldScale;


    public void Init() {
        oldHeight = scene.transform.localPosition.y;
        oldScale = scene.transform.localScale;
    }

    public void UpdateScale(float scaleMult) {
        Vector3 newScale = oldScale * scaleMult;
        newScale.x = Mathf.Clamp(newScale.x,0.25f,2f);
        newScale.y = Mathf.Clamp(newScale.y,0.25f,2f);
        newScale.z = Mathf.Clamp(newScale.z,0.25f,2f);
        scene.transform.localScale = newScale;
    }

    public void RevertScale(Slider slider) {
        slider.value = 1;
    }

    public void UpdateHeight(float height) {
        scene.transform.localPosition = new Vector3(scene.transform.localPosition.x,oldHeight + height,scene.transform.localPosition.z);
    }

    public void RevertHeight(Slider slider) {
        slider.value = oldHeight;
    }

    public void StartSwap() {
        //ARController.canSwap = true;
    }

    public void CancelSwap() {
        //scene.transform.localScale = oldTransform.localScale;
        //scene.transform.localRotation = oldTransform.localRotation;
    }

    public void ConfirmUpdate() {
        //ARController.canSwap = false;
    }
}
