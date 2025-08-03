using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Enemy enemy = (Enemy)target;

        // Draw default fields except isType, maxRangeX, minRangeX, maxRangeSodor, minRangeSodor
        DrawPropertiesExcluding(serializedObject, "isType", "maxRangeX", "minRangeX", "maxRangeSodor", "minRangeSodor", "moveSpeed");

        enemy.moveSpeed = EditorGUILayout.FloatField("Move Spedd", enemy.moveSpeed);
        
        EditorGUILayout.Space(10);
        // Radio button group
        GUILayout.Label("Mode", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        int mode = enemy.isType ? 1 : 0;
        mode = GUILayout.Toolbar(mode, new string[] { "Gobak", "Sodor" }, "Radio");

        enemy.isType = (mode == 1);

        EditorGUILayout.EndHorizontal();

        // Show fields based on mode
        if (!enemy.isType)
        {
            enemy.maxRangeX = EditorGUILayout.FloatField("Max Range X", enemy.maxRangeX);
            enemy.minRangeX = EditorGUILayout.FloatField("Min Range X", enemy.minRangeX);
        }
        else
        {
            enemy.maxRangeSodor = EditorGUILayout.FloatField("Max Range Sodor", enemy.maxRangeSodor);
            enemy.minRangeSodor = EditorGUILayout.FloatField("Min Range Sodor", enemy.minRangeSodor);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(enemy);
    }
}