using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question",menuName = "ScriptableObjects/StoryManager/StepQuestion",order = 2)]
public class Question : ScriptableObject {
    public string question;
    public QuestionType qType;
    public int anser;
    public string[] choices;
    public GameObject answerLayout;

    public enum QuestionType {
        MultiChoice,
        Numpad
    }
}
