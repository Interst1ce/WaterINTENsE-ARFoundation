using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BioreactorResultsIntro : MonoBehaviour {
    public List<AudioClip> audioSrc;
    public Image paper;
    public Image monitor;
    public GameObject resultsPopup;
    public List<Sprite> monitorScreens;
    public Material glowMat;
    AudioSource audioSource;
    bool step1, step2, audio1;

    void Start() {
        step1 = false;
        step2 = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSrc[0]);
        Glow(paper,audioSrc[0].length);
    }

    public async void Glow(Image image, float delay) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        image.material = glowMat;
    }

    public void UnGlow(Image image) {
        image.material = null;
        image.material = Image.defaultGraphicMaterial;
    }

    public async void StepOne() {
        if (!step1) {
            step1 = true;
            resultsPopup.SetActive(true);
            UnGlow(paper);
            audioSource.PlayOneShot(audioSrc[1]);
            Glow(monitor,audioSrc[1].length);
            await Task.Delay(TimeSpan.FromSeconds(audioSrc[1].length));
            audio1 = true;
        }
    }

    public async void StepTwo() {
        if (!step2 && step1 && audio1) {
            step2 = true;
            resultsPopup.SetActive(false);
            UnGlow(monitor);
            monitor.sprite = monitorScreens[1];
            audioSource.PlayOneShot(audioSrc[2]);
            await Task.Delay(TimeSpan.FromSeconds(audioSrc[2].length));
            audioSource.PlayOneShot(audioSrc[3]);
            await Task.Delay(TimeSpan.FromSeconds(audioSrc[3].length));
            SceneTransition();
        }
    }

    public void SceneTransition() {
        //fade to black?
        GetComponent<SceneLoader>().LoadScene(5);
    }
}
 