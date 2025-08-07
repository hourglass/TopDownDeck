using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MotionData))]
public class MotionDataEditor : Editor
{
    private static readonly string[] directionLabels = { "E", "N", "NE", "S", "SE" };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // animations �迭 ó��
        SerializedProperty animations = serializedObject.FindProperty("animations");
        if (animations.arraySize != 5)
        {
            animations.arraySize = 5; // �迭 ũ�� 5�� ����
        }

        // �� �迭 ��ҿ� ���� ���̺� ǥ��
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