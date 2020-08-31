using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StepO",menuName = "ScriptableObjects/StoryManager/Step",order = 0)]
public class Step : ScriptableObject {
    public List<Target> targets;
    public List<Question> questions;
}
