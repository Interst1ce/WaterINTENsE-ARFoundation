using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Step",menuName = "ScriptableObjects/StoryManager/Step",order = 0)]
public class Step : ScriptableObject {
    public List<Target> targets = new List<Target>();
    public List<Question> questions = new List<Question>();

    public void AddTarget(Target target) {
        targets.Add(target);
    }

    public void AddQuestion(Question question) {
        questions.Add(question);
    }
}
