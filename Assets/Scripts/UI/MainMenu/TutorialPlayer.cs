using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Google.Play.AssetDelivery;

public class TutorialPlayer : MonoBehaviour {
    [SerializeField]
    GameObject tutorialBackground;

    AssetBundle asset;

    VideoPlayer video;
    RawImage vidTex;
    GraphicRaycaster graphicRaycaster;

    private void Awake() {
        video = tutorialBackground.GetComponentInChildren<VideoPlayer>();
        vidTex = tutorialBackground.GetComponentInChildren<RawImage>();
        graphicRaycaster = tutorialBackground.GetComponentInParent<GraphicRaycaster>();
        asset = PlayAssetDelivery.RetrieveAssetBundleAsync("tutorialvideo").AssetBundle;
        tutorialBackground.SetActive(false);
    }

    public void StartTutorial() {
        video.clip = asset.LoadAssetAsync<VideoClip>("TutorialVideoSound.mp4").asset as VideoClip;
        tutorialBackground.SetActive(true);
        video.Play();
    }

    public void ExitTutorial() {
        video.Stop();
        asset.Unload(true);
        tutorialBackground.SetActive(false);
    }

    public void PlayPause() {
        if (video.isPlaying) {
            video.Pause();
        } else video.Play();
    }
}
