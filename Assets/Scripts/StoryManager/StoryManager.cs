using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        highlightManager = GetComponent<HighlightManager>();
    }

    private async Task PopulateTargetDictionary() {
        for(int i = 0; i < steps.Count; i++) {
            objectTargets.Add(i,new List<GameObject>());
            foreach (Target target in steps[i].targets) objectTargets[i].Add(GameObject.Find(target.objectTarget));
            foreach (GameObject target in objectTargets[i]) Debug.Log("" + target);
            await Task.Yield();
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
                        Debug.Log("" + interactionMatch);
                        for(int j = 0; j < steps[currentStep].targets.Count - 1; j++) {
                            Debug.Log("Checking if valid object was tapped");
                            if(hit.transform.gameObject == objectTargets[currentStep][j] && interactionMatch) {
                                Debug.Log("Continuing story");
                                ContinueStory(target);
                            }
                            Debug.Log("User didn't tap a valid object");
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

    public void PlaySFX(AudioClip audio) {
        if(audio != null) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.PlayOneShot(audio);
            Destroy(source,audio.length);
        }
    }

    public async void StartStory() {
        await PopulateTargetDictionary();
        PlayAudio(introAudio);
        await Task.Delay(TimeSpan.FromSeconds(introAudio.length));
        foreach (GameObject target in objectTargets[0]) {
            Debug.Log("" + target);
            if (target != null) highlightManager.StartGlow(target);
        }
        currentStep = 0;
    }

    void ContinueStory(Target target) {
        if (!audioSource.isPlaying) {
            highlightManager.glow = false;
            PlayAudio(target.targetAudio);
            //play animation
            //run any extensions
            currentStep++;
        }
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
