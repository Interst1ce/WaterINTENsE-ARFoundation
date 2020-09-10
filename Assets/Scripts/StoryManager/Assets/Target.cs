using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Target",menuName = "ScriptableObjects/StoryManager/StepTarget",order = 1)]
public class Target : ScriptableObject {
    public TargetType type;
    public Interaction interaction;
    public string objectTarget;
    public Slider sliderTarget;
    public int targetStep;
    public AnimationClip targetAnim;
    public AudioClip targetAudio;
    public bool playAudioAfterAnim;

    public enum TargetType {
        Object,
        Slider_NotImplementedYet
    }

    public enum Interaction {
        Tap,
        SwipeUp,
        SwipeDown,
        SwipeLeft,
        SwipeRight
    }
}
