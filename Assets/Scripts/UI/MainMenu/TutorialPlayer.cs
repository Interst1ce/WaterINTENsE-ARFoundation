using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialPlayer : MonoBehaviour {
    [SerializeField]
    GameObject tutorialBackground;

    VideoPlayer video;
    RawImage vidTex;
    GraphicRaycaster graphicRaycaster;

    private void Awake() {
        video = tutorialBackground.GetComponentInChildren<VideoPlayer>();
        vidTex = tutorialBackground.GetComponentInChildren<RawImage>();
        graphicRaycaster = tutorialBackground.GetComponentInParent<GraphicRaycaster>();
        tutorialBackground.SetActive(false);
    }

    public void StartTutorial() {
        tutorialBackground.SetActive(true);
        video.Play();
    }

    public void ExitTutorial() {
        video.Stop();
        tutorialBackground.SetActive(false);
    }

    public void PlayPause() {
        if (video.isPlaying) {
            video.Pause();
        } else video.Play();
    }
}
