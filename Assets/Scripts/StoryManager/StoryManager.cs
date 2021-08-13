﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(HighlightManager))]
[RequireComponent(typeof(AudioListener))]
public class StoryManager : MonoBehaviour {
    public int currentStep;
    //bool introPlayed = false;
    bool finished = false;
    public AudioSource audioSource;
    HighlightManager highlightManager;

    

    [SerializeField]
    bool reviewMode = false;
    [SerializeField]
    AudioClip introAudio;
    [SerializeField]
    UnityEvent introAnim;
    [SerializeField]
    AudioClip backgroundSFX;
    [SerializeField]
    AudioClip missTapAudio;
    public List<AudioSource> loopingSFX = new List<AudioSource>();

    [HideInInspector]
    public List<NewTarget> newTargets;

    [Space]
    public List<StepExt> steps = new List<StepExt>();
    Dictionary<int,List<GameObject>> objectTargets = new Dictionary<int,List<GameObject>>();

    bool interactionMatch;

    Animator lastAnimator;
    AnimationClip lastAnim;
    QuestionManager qManager;

    [Serializable]
    public struct StepExt {
        public Step step;
        public UnityEvent extensions;
    }

    [Serializable]
    public struct NewTarget {
        public Target.TargetType type;
        public Target.Interaction interaction;
        public string objectTarget;
        public Slider sliderTarget;
        public int targetStep;
        public AnimationClip targetAnim;
        public AudioClip targetAudio;
        public bool playAudioAfterAnim;
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        highlightManager = GetComponent<HighlightManager>();
        qManager = GetComponentInChildren<QuestionManager>();
    }

    private async Task PopulateTargetDictionary() {
        for (int i = 0; i < steps.Count; i++) {
            objectTargets.Add(i,new List<GameObject>());
            foreach (Target target in steps[i].step.targets) objectTargets[i].Add(GameObject.Find(target.objectTarget));
            await Task.Yield();
        }
    }

    void Update() {

        if (PauseMenu.paused) return;
        if (qManager != null) {
            if (!qManager.inQuestion) {
                for (int i = 0; i < Input.touchCount; i++) {
                    Touch tap = Input.GetTouch(i);
                    if (tap.phase == TouchPhase.Began) {
                        RaycastHit hit;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(tap.position),out hit)) {
                            foreach (Target target in steps[currentStep].step.targets) {
                                interactionMatch = false;
                                StartCoroutine(DetectInput(target.interaction,tap.position));
                                for (int j = 0; j < steps[currentStep].step.targets.Count; j++) {
                                    if (hit.transform.gameObject == objectTargets[currentStep][j] && interactionMatch) {
                                        /*if (target.targetStep == steps.Count && currentStep == steps.Count) {
                                            finished = true;
                                            EndStory(target);
                                        } else ContinueStory(target);*/
                                        ContinueStory(target);
                                    } else {
                                        if (!audioSource.isPlaying) {
                                            PlaySFX(missTapAudio);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //if(steps[currentStep - 1].step.targets[0].targetStep == steps.Count && currentStep == steps.Count) {
        //    EndStory(steps[currentStep].step.targets[0]);
        //}
    }

    public async void StartStory() {
        await PopulateTargetDictionary();
        if (!reviewMode) {
            PlayAudio(introAudio);
            introAnim.Invoke();
            await Task.Delay(TimeSpan.FromSeconds(introAudio.length));
        }
        foreach (GameObject target in objectTargets[0]) {
            if (target != null) highlightManager.StartGlow(target);
        }
        currentStep = 0;
    }

    async void ContinueStory(Target target) {
        //Basically, when 'ContinueStory' gets called, this event gets invoked--functions with custom functionality can be attached to this event, so that they get run here
        steps[currentStep].extensions.Invoke();
        while (qManager.inQuestion) await Task.Yield();
        if (!audioSource.isPlaying && (lastAnim == null || (lastAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !lastAnimator.IsInTransition(0)))) {
            highlightManager.glow = false;
            if (!reviewMode) {
                if (!target.playAudioAfterAnim || target.targetAnim == null) {
                    PlayAudio(target.targetAudio);
                } else {
                    PlayAudio(target.targetAudio,target.targetAnim.length);
                }
            }
            //Animation code
            Animator targetAnimator = GetTargetAnimator(GameObject.Find(target.objectTarget));
            lastAnimator = targetAnimator;
            lastAnim = target.targetAnim;
            if (targetAnimator != null && target.targetAnim != null) targetAnimator.Play(target.targetAnim.name);
            //steps[currentStep].extensions.Invoke();
            List<GameObject> highlightTargets = new List<GameObject>();
            if (currentStep < steps.Count - 1) {
                foreach (Target nextTarget in steps[currentStep + 1].step.targets) {
                    highlightTargets.Add(GameObject.Find(nextTarget.objectTarget));
                }
            }
            if (target.targetAnim != null && !reviewMode) {
                if (target.targetAudio != null) {
                    highlightManager.StartGlow(highlightTargets,Mathf.Max(target.targetAnim.length,target.targetAudio.length));
                } else highlightManager.StartGlow(highlightTargets,target.targetAnim.length);
            } else if (target.targetAudio != null) {
                highlightManager.StartGlow(highlightTargets,target.targetAudio.length);
            } else highlightManager.StartGlow(highlightTargets,1f);
            currentStep = target.targetStep;
            if (currentStep == steps.Count) EndStory(target);
        }
    }

    async public void EndStory(Target target) {
        finished = true;
        float seconds = Mathf.Max(target.targetAudio.length,target.targetAnim.length);
        int delay = Mathf.FloorToInt(seconds * 1000) + Mathf.FloorToInt(seconds % 1 * 1000);

        //Debug.Log("Waiting: " + delay);
        await Task.Delay(delay);

        GameObject.Find("PauseUI").GetComponent<PauseMenu>().Pause();
    }

    public void PlayAudio(AudioClip audio,float delay = 0) {
        if (audio != null) {
            audioSource.clip = audio;
            audioSource.PlayDelayed(delay);
        }
    }

    public void PlaySFX(AudioClip audio,bool loop = false) {
        if (audio != null) {
            if (loop) {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.clip = audio;
                source.loop = loop;
                source.Play();
                loopingSFX.Add(source);
                //Destroy(source,audio.length);
            } else {
                audioSource.PlayOneShot(audio);
            }
        }
    }

    Animator GetTargetAnimator(GameObject target) {
        Animator animator = target.GetComponent<Animator>();
        if (animator == null) animator = target.GetComponentInParent<Animator>();
        if (animator == null) animator = target.transform.parent.GetComponentInParent<Animator>();
        return animator;
    }

    IEnumerator DetectInput(Target.Interaction toCheck,Vector2 startPos) {
        float startTime = Time.time;
        float timeDelta = 0;

        Vector2 curPos;
        Vector2 posDelta;

        do {
            timeDelta = Time.time - startTime;
            if (toCheck == Target.Interaction.Tap) {
                yield return interactionMatch = true;
                break;
            } else {
                curPos = Input.GetTouch(0).position;
                posDelta = curPos - startPos;
                if (Mathf.Abs(posDelta.x) > 1) {
                    if (Mathf.Sign(posDelta.x) == -1) {
                        yield return interactionMatch = true;
                    } else if (Mathf.Sign(posDelta.x) == 1) {
                        yield return interactionMatch = true;
                    }
                } else if (Mathf.Abs(posDelta.y) > 1) {
                    if (Mathf.Sign(posDelta.y) == -1) {
                        yield return interactionMatch = true;
                    } else if (Mathf.Sign(posDelta.y) == 1) {
                        yield return interactionMatch = true;
                    }
                }
            }
        } while (timeDelta < 0.5f);
    }

}
