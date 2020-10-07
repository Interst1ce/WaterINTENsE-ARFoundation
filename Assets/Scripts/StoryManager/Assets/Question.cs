using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question",menuName = "ScriptableObjects/StoryManager/Extensions/Question",order = 0)]
public class Question : ScriptableObject {
    public string question;
    public int step;
    public QuestionType qType;
    public int answer;
    [Header("Try to keep # of answers < 5")]
    public string[] choices;
    [Header("Answer layout prefab must have same number of buttons as number of choices")]
    public GameObject answerLayout;
}

public enum QuestionType {
    MultiChoice,
    Numpad
}