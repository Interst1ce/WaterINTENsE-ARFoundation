using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Module",menuName = "ScriptableObjects/Module",order = 3)]
public class Module : ScriptableObject {
    [Header("Make sure every variable is assigned or it won't work")]
    public List<ChapterData> chapters = new List<ChapterData>();
}

[System.Serializable]
public struct ChapterData {
    public string chapterTitle;
    public string chapterSummary;
    public Sprite chapterIcon;
    public int chapterScene;
}
