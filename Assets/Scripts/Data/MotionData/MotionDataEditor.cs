using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MotionData))]
public class MotionDataEditor : Editor
{
    private static readonly string[] directionLabels = { "E", "N", "NE", "S", "SE" };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // animations 배열 처리
        SerializedProperty animations = serializedObject.FindProperty("animations");
        if (animations.arraySize != 5)
        {
            animations.arraySize = 5; // 배열 크기 5로 고정
        }

        // 각 배열 요소에 방향 레이블 표시
        EditorGUILayout.LabelField("Animations (5 Directions)");
        EditorGUI.indentLevel++;
        for (int i = 0; i < animations.arraySize; i++)
        {
            string label = i < directionLabels.Length ? directionLabels[i] : $"Animation {i}";
            EditorGUILayout.PropertyField(animations.GetArrayElementAtIndex(i), new GUIContent(label), true);
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}