using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardTextureUpdate : MonoBehaviour {
    [SerializeField]
    Renderer clipboardRenderer;
    [SerializeField]
    Texture newTexture;

    bool questionState;

    private void Update() {
        if (!QuestionManagerV2_1.inQuestion && questionState) clipboardRenderer.material.SetTexture("_MainTex",newTexture);
        questionState = QuestionManagerV2_1.inQuestion;
    }
}
