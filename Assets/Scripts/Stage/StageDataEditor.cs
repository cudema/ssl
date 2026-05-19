using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageData))]
public class StageDataEditor : Editor
{
    SerializedProperty stageType;
    SerializedProperty enmeyGroup;
    SerializedProperty waveDilayTime;
    SerializedProperty dropCoin;
    SerializedProperty restStageData;
    SerializedProperty shopStageData;
    SerializedProperty treasureStageData;


    void OnEnable()
    {
        stageType = serializedObject.FindProperty("stageType");
        enmeyGroup = serializedObject.FindProperty("enmeyGroup");
        waveDilayTime = serializedObject.FindProperty("waveDilayTime");
        dropCoin = serializedObject.FindProperty("dropCoin");
        restStageData = serializedObject.FindProperty("restStageData");
        shopStageData = serializedObject.FindProperty("shopStageData");
        treasureStageData = serializedObject.FindProperty("treasureStageData");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        //StageData data = (StageData)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(stageType, true);

        switch (stageType.enumValueIndex)
        {
            case (int)StageType.Combat:
                EditorGUILayout.PropertyField(enmeyGroup, true);
                EditorGUILayout.PropertyField(waveDilayTime, true);
                EditorGUILayout.PropertyField(dropCoin, true);
                break;
            // case (int)StageType.Event:
            //     break;
            case (int)StageType.Shop:
                EditorGUILayout.PropertyField(shopStageData, true);
                break;
            case (int)StageType.Treasure:
                EditorGUILayout.PropertyField(treasureStageData, true);
                break;
            case (int)StageType.Rest:
                EditorGUILayout.PropertyField(restStageData, true);
                break;
            case (int)StageType.Boss:
                EditorGUILayout.PropertyField(enmeyGroup, true);
                EditorGUILayout.PropertyField(waveDilayTime, true);
                EditorGUILayout.PropertyField(dropCoin, true);
                break;
            case (int)StageType.Elite:
                EditorGUILayout.PropertyField(enmeyGroup, true);
                EditorGUILayout.PropertyField(treasureStageData, true);
                EditorGUILayout.PropertyField(waveDilayTime, true);
                EditorGUILayout.PropertyField(dropCoin, true);
                break;
            // case (int)StageType.Smithy:
            //     break;
            default :
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
