using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(StoryManager))]
public class StoryManagerEditor : Editor {

    enum displayFieldType { DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields }
    displayFieldType DisplayFieldType;

    StoryManager t;
    SerializedObject getTarget;
    SerializedProperty thisList;
    int listSize;

    void OnEnable() {
        t = (StoryManager)target;
        getTarget = new SerializedObject(t);
        thisList = getTarget.FindProperty("newTargets");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        getTarget.Update();

        var headerStyle = new GUIStyle(GUI.skin.label);
        headerStyle.alignment = TextAnchor.MiddleCenter;
        headerStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Step Creation", headerStyle, GUILayout.ExpandWidth(true));

        if (GUILayout.Button("Add Target")) {
            t.newTargets.Add(new StoryManager.NewTarget());
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();


        //Display our list to the inspector window
        for (int i = 0; i < thisList.arraySize; i++) {
            SerializedProperty listRef = thisList.GetArrayElementAtIndex(i);
            SerializedProperty ntType = listRef.FindPropertyRelative("type");
            SerializedProperty ntInteraction = listRef.FindPropertyRelative("interaction");
            SerializedProperty ntObjectTarget = listRef.FindPropertyRelative("objectTarget");
            SerializedProperty ntSliderTarget = listRef.FindPropertyRelative("sliderTarget");
            SerializedProperty ntTargetStep = listRef.FindPropertyRelative("targetStep");
            SerializedProperty ntTargetAnim = listRef.FindPropertyRelative("targetAnim");
            SerializedProperty ntTargetAudio = listRef.FindPropertyRelative("targetAudio");
            SerializedProperty ntAudioAfterAnim = listRef.FindPropertyRelative("playAudioAfterAnim");

            if (GUILayout.Button("Remove Target")) {
                thisList.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (DisplayFieldType == 0) {
                EditorGUILayout.PropertyField(ntType);
                EditorGUILayout.PropertyField(ntInteraction);
                if(ntInteraction.enumValueIndex == 0) {
                    EditorGUILayout.PropertyField(ntObjectTarget);
                }else EditorGUILayout.PropertyField(ntSliderTarget);
                EditorGUILayout.PropertyField(ntTargetStep);
                EditorGUILayout.PropertyField(ntTargetAnim);
                EditorGUILayout.PropertyField(ntTargetAudio);
                EditorGUILayout.PropertyField(ntAudioAfterAnim);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        //Apply the changes to our list
        getTarget.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Step")) {
            string sceneName = EditorSceneManager.GetActiveScene().name;
            int i = 1;
            Step newStep = CreateInstance<Step>();
            StoryManager.StepExt newStepExt = new StoryManager.StepExt();
            newStepExt.step = newStep;
            t.steps.Add(newStepExt);

            string stepsDirectoryPath = "Assets/Scenes/" + sceneName + "/Steps/";
            string targetsDirectoryPath = "Assets/Scenes/" + sceneName + "/Steps/Targets/";
            System.IO.Directory.CreateDirectory(stepsDirectoryPath);
            System.IO.Directory.CreateDirectory(targetsDirectoryPath);

            AssetDatabase.CreateAsset(newStep,stepsDirectoryPath + $"S{t.steps.Count}_{sceneName}.asset");
            Step step = (Step)AssetDatabase.LoadAssetAtPath(System.IO.Path.Combine(stepsDirectoryPath + $"S{t.steps.Count}_{sceneName}.asset"),typeof(Step));

            foreach (StoryManager.NewTarget target in t.newTargets) {
                Target newTarget = CreateInstance<Target>();
                newTarget.type = target.type;
                newTarget.interaction = target.interaction;
                newTarget.objectTarget = target.objectTarget;
                newTarget.sliderTarget = target.sliderTarget;
                newTarget.targetStep = target.targetStep;
                newTarget.targetAnim = target.targetAnim;
                newTarget.targetAudio = target.targetAudio;
                newTarget.playAudioAfterAnim = target.playAudioAfterAnim;

                AssetDatabase.CreateAsset(newTarget,targetsDirectoryPath + $"S{t.steps.Count}_T{i}_{sceneName}.asset");
                Target addTarget = (Target)AssetDatabase.LoadAssetAtPath(System.IO.Path.Combine(targetsDirectoryPath,$"S{t.steps.Count}_T{i}_{sceneName}.asset"),typeof(Target));
                step.AddTarget(addTarget);
                EditorUtility.SetDirty(step);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                i++;
            }
            
            t.newTargets.Clear();
            getTarget.ApplyModifiedProperties();
        }
        //DisplayFieldType = (displayFieldType)EditorGUILayout.EnumPopup("",DisplayFieldType);
    }
}
