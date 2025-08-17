using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    SerializedProperty moveSpeedProp;
    SerializedProperty isTypeProp;
    SerializedProperty maxRangeXProp;
    SerializedProperty minRangeXProp;
    SerializedProperty maxRangeSodorProp;
    SerializedProperty minRangeSodorProp;

    private void OnEnable()
    {
        moveSpeedProp = serializedObject.FindProperty("moveSpeed");
        isTypeProp = serializedObject.FindProperty("isType");
        maxRangeXProp = serializedObject.FindProperty("maxRangeX");
        minRangeXProp = serializedObject.FindProperty("minRangeX");
        maxRangeSodorProp = serializedObject.FindProperty("maxRangeSodor");
        minRangeSodorProp = serializedObject.FindProperty("minRangeSodor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Enemy enemy = (Enemy)target;

        // Show move speed
        EditorGUILayout.PropertyField(moveSpeedProp);

        EditorGUILayout.Space(10);
        GUILayout.Label("Mode", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        int mode = isTypeProp.boolValue ? 1 : 0;
        mode = GUILayout.Toolbar(mode, new string[] { "Gobak", "Sodor" }, "Radio");
        isTypeProp.boolValue = (mode == 1);
        EditorGUILayout.EndHorizontal();

        // Show relevant ranges
        if (!isTypeProp.boolValue)
        {
            EditorGUILayout.PropertyField(maxRangeXProp);
            EditorGUILayout.PropertyField(minRangeXProp);
        }
        else
        {
            EditorGUILayout.PropertyField(maxRangeSodorProp);
            EditorGUILayout.PropertyField(minRangeSodorProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
