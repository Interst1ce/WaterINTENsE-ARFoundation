using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HighlightManager))]
[RequireComponent(typeof(AudioListener))]
public class StoryManager : MonoBehaviour {
    public int currentStep;
    bool introPlayed = false;
    bool finished = false;
    AudioSource audioSource;
    HighlightManager highlightManager;

    [SerializeField]
    AudioClip introAudio;
    [SerializeField]
    AudioClip introSFX;
    [SerializeField]
    AudioClip missTapAudio;

    public List<Step> steps = new List<Step>();
    Dictionary<int,List<GameObject>> objectTargets = new Dictionary<int,List<GameObject>>();

    bool interactionMatch;

    private async void Awake() {
        audioSource = GetComponent<AudioSource>();
        highlightManager = GetComponent<HighlightManager>();
        await PopulateTargetDictionary();
        GameObject.Find("EventSystem").GetComponent<StoryManager>().StartStory();
    }

    private async Task PopulateTargetDictionary() {
        for(int i = 0; i < steps.Count; i++) {
            objectTargets.Add(i,new List<GameObject>());
            foreach (Target target in steps[i].targets) objectTargets[i].Add(GameObject.Find(target.objectTarget));
            foreach (GameObject target in objectTargets[i]) Debug.Log("" + target);
        }
    }

    void Update() {
        //EndStory();
        for (int i = 0; i < Input.touchCount; i++) {
            Touch tap = Input.GetTouch(i);
            if (tap.phase == TouchPhase.Began) {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(tap.position),out hit)) {
                    foreach (Target target in steps[currentStep].targets) {
                        interactionMatch = false;
                        StartCoroutine(DetectInput(target.interaction,tap.position));
                        for(int j = 0; j < steps[currentStep].targets.Count - 1; j++) {
                            if(hit.transform.gameObject == objectTargets[currentStep][j] && interactionMatch) {
                                ContinueStory(target);
                            }
                        }
                    }
                }
            }
        }
    }

    public void PlayAudio(AudioClip audio) {
        if(audio != null) {
            audioSource.clip = audio;
            audioSource.Play();
        }
    }

    public async void StartStory() {
        PlayAudio(introAudio);
        await Task.Delay(TimeSpan.FromSeconds(introAudio.length));
        foreach (GameObject target in objectTargets[0]) {
            Debug.Log("" + target);
            if (target != null) highlightManager.StartGlow(target);
        }
    }

    void ContinueStory(Target target) {
        highlightManager.glow = false;
        //play animation
        //play naration/sfx
        //run any extensions
        //increment the step counter
    }

    void EndStory() {
        if (currentStep == steps.Count && !audioSource.isPlaying && !finished) {
            finished = true;
            if (steps[currentStep]) {
                //pause the story
            }
        }
    }

    IEnumerator DetectInput(Target.Interaction toCheck, Vector2 startPos) {
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
