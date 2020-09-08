using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiAnimTrigger",menuName = "ScriptableObjects/StoryManager/Extensions/MultiAnimTrigger",order = 1)]
public class MultiAnimTrigger : ScriptableObject {
    public List<AnimTriggerData> anims = new List<AnimTriggerData>();
}
[System.Serializable]
public struct AnimTriggerData {
    public string targetObjPath;
    public string animTrigger;
    public float delay;
}