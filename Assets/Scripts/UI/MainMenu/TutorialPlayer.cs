using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Google.Play.AssetDelivery;

public class TutorialPlayer : MonoBehaviour {
    [SerializeField]
    GameObject tutorialBackground;

    PlayAssetBundleRequest bundleRequest;
    AssetBundle asset;

    VideoPlayer video;
    RawImage vidTex;
    GraphicRaycaster graphicRaycaster;

    private async void Awake() {
        video = tutorialBackground.GetComponentInChildren<VideoPlayer>();
        vidTex = tutorialBackground.GetComponentInChildren<RawImage>();
        graphicRaycaster = tutorialBackground.GetComponentInParent<GraphicRaycaster>();
        tutorialBackground.SetActive(false);
        bundleRequest = PlayAssetDelivery.RetrieveAssetBundleAsync("tutorialvideo");
        while (!bundleRequest.IsDone) {
            await Task.Yield();
        }
        asset = bundleRequest.AssetBundle;
    }

    public void StartTutorial() {
        if (bundleRequest.IsDone) {
            VideoClip clip = asset.LoadAsset<VideoClip>("tutorialvideosound.mp4");
            tutorialBackground.SetActive(true);
            video.clip = clip;
            video.Play();
        }
    }

    public void ExitTutorial() {
        video.Stop();
        tutorialBackground.SetActive(false);
    }

    public void AssetBundleCleanup() {
        asset.Unload(true);
    }

    public void PlayPause() {
        if (video.isPlaying) {
            video.Pause();
        } else video.Play();
    }
}
