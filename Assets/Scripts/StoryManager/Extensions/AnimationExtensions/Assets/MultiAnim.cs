using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiAnim",menuName = "ScriptableObjects/StoryManager/Extensions/MultiAnim",order = 0)]
public class MultiAnim : ScriptableObject {
    public List<AnimData> anims = new List<AnimData>();    
}
[System.Serializable]
public struct AnimData {
    public string targetObjPath;
    public string animTitle;
    public float delay;
}
