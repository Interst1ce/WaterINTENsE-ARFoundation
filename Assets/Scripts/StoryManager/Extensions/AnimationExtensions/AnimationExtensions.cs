using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class AnimationExtensions : MonoBehaviour {

    [SerializeField]
    GameObject tag;
    [SerializeField]
    GameObject padLock;
    [SerializeField]
    GameObject padLockPin;


    [SerializeField]
    GameObject tagPrefab;
    [SerializeField]
    GameObject lockPrefab;
    [SerializeField]
    GameObject padLockPinPrefab;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    PauseMenu pauseMenu;

    [SerializeField]
    Text debugText;

    [SerializeField]
    Animator padlockAnimator;

    bool moduleEnd;

    public void PlayMultiAnim(MultiAnim multAnim) {
        foreach (AnimData anim in multAnim.anims) {
            GameObject target = GameObject.Find(anim.targetObjPath);
            Animator anmat = target.GetComponent<Animator>();
            if (anmat != null) {
                DelayAnim(anmat,anim);
            } else {
                anmat = target.GetComponentInParent<Animator>();
                if (anmat != null) {
                    DelayAnim(anmat,anim);
                } else {
                    anmat = target.transform.parent.GetComponentInParent<Animator>();
                    DelayAnim(anmat,anim);
                }
            }
        }
    }
    public void PlayMultiAnim(MultiAnimTrigger multAnim) {
        foreach (AnimTriggerData anim in multAnim.anims) {
            GameObject target = GameObject.Find(anim.targetObjPath);
            Animator anmat = target.GetComponent<Animator>();
            if (anmat != null) {
                DelayAnim(anmat,anim);
            } else {
                anmat = target.GetComponentInParent<Animator>();
                if (anmat != null) {
                    DelayAnim(anmat,anim);
                } else {
                    anmat = target.transform.parent.GetComponentInParent<Animator>();
                    DelayAnim(anmat,anim);
                }
            }
        }
    }

    public async void DelayAnim(Animator anmat,AnimData anim) {
        await Task.Delay(TimeSpan.FromSeconds(anim.delay));
        anmat.Play(anim.animTitle);
    }

    public async void DelayAnim(Animator anmat,AnimTriggerData anim) {
        await Task.Delay(TimeSpan.FromSeconds(anim.delay));
        anmat.SetTrigger(anim.animTrigger);
    }

    //Extra logic for slider and other stuff
    public void DisableLockTagMesh() {
        StartCoroutine(StartDisableLockTagMesh());
        //StartCoroutine(AudioDelay());
    }

    public void DisableLockTagMeshLotoValve() {
        //debugText.text = "In StartDisable";

        tag.gameObject.GetComponent<MeshRenderer>().enabled = false;
        padLock.gameObject.GetComponent<MeshRenderer>().enabled = false;
        padLockPin.gameObject.GetComponent<MeshRenderer>().enabled = false;

        //debugText.text = "tag/lock disabled";

        tagPrefab.gameObject.GetComponent<MeshRenderer>().enabled = true;
        lockPrefab.gameObject.GetComponent<MeshRenderer>().enabled = true;
        padLockPinPrefab.gameObject.GetComponent<MeshRenderer>().enabled = true;

        StartCoroutine(EndLotoValveScene());
    }
    //this is for LOTOCB
    public IEnumerator StartDisableLockTagMesh() {
        yield return new WaitForSeconds(3);
        //debugText.text = "In StartDisable";

        tag.gameObject.GetComponent<MeshRenderer>().enabled = false;
        padLock.gameObject.GetComponent<MeshRenderer>().enabled = false;
        padLockPin.gameObject.GetComponent<MeshRenderer>().enabled = false;

        //debugText.text = "tag/lock disabled";

        tagPrefab.gameObject.GetComponent<MeshRenderer>().enabled = true;
        lockPrefab.gameObject.GetComponent<MeshRenderer>().enabled = true;
        padLockPinPrefab.gameObject.GetComponent<MeshRenderer>().enabled = true;

        StartCoroutine(EndLotoCBScene());
    }
    //For LotoValve

    public void StartLockTagPrefabGlow() {
        //this.GetComponent<HighlightManager>().StartGlow(lockPrefab);

        //StartCoroutine(LotoValveLockTagPrefabGlow());
    }
    public IEnumerator LotoValveLockTagPrefabGlow() {
        try {
            if (padlockAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !padlockAnimator.IsInTransition(0) && !audioSource.isPlaying) {
                this.GetComponent<HighlightManager>().StartGlow(lockPrefab);
            }
        } catch {
            //this is getting called as of build 26
            //debugText.text = "LotoValveLockTagPrefabGlow() Not working correctly";
        }
        yield return null;
    }

    public IEnumerator EndLotoCBScene() {
        while (true) {
            if (!audioSource.isPlaying) {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(2);
        pauseMenu.Pause();
    }

    public IEnumerator EndLotoValveScene() {
        debugText.text = "In EndLotoValveScene()";
        yield return new WaitForSeconds(6);

        while (true) {
            if (!audioSource.isPlaying) {
                pauseMenu.Pause();
                break;
            }
            yield return null;
        }
    }

    public void EndPPE() {
        StartCoroutine(EndPPECoroutine());
    }

    public IEnumerator EndPPECoroutine() {
        while (true) {
            if (!audioSource.isPlaying) {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(44f);
        pauseMenu.Pause();
    }

    
    /*public IEnumerator StartDisableLockTagMeshLotoValve()
    {
        //yield return new WaitForSeconds(3);
        debugText.text = "In StartDisable";


        tag.gameObject.GetComponent<MeshRenderer>().enabled = false;
        padLock.gameObject.GetComponent<MeshRenderer>().enabled = false;
        padLockPin.gameObject.GetComponent<MeshRenderer>().enabled = false;

        debugText.text = "tag/lock disabled";

        tagPrefab.gameObject.GetComponent<MeshRenderer>().enabled = true;
        lockPrefab.gameObject.GetComponent<MeshRenderer>().enabled = true;
        padLockPinPrefab.gameObject.GetComponent<MeshRenderer>().enabled = true;

    }*/
}
