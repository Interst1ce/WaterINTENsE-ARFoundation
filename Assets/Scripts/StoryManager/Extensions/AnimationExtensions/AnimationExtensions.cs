using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AnimationExtensions : MonoBehaviour {
    public void PlayMultiAnim(MultiAnim multAnim) {
        foreach(AnimData anim in multAnim.anims) {
            GameObject target = GameObject.Find(anim.targetObjPath);
            Animator anmat = target.GetComponent<Animator>();
            if (anmat != null) DelayAnim(anmat,anim);
        }
    }
    public void PlayMultiAnim(MultiAnimTrigger multAnim) {
        foreach (AnimTriggerData anim in multAnim.anims) {
            GameObject target = GameObject.Find(anim.targetObjPath);
            Animator anmat = target.GetComponent<Animator>();
            if (anmat != null) DelayAnim(anmat,anim);
        }
    }

    public async void DelayAnim(Animator anmat, AnimData anim) {
        await Task.Delay(TimeSpan.FromSeconds(anim.delay));
        anmat.Play(anim.animTitle);
    }

    public async void DelayAnim(Animator anmat, AnimTriggerData anim) {
        await Task.Delay(TimeSpan.FromSeconds(anim.delay));
        anmat.SetTrigger(anim.animTrigger);
    }
}
