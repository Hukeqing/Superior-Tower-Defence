using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExtraUIControl))]
public class InspectorButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExtraUIControl myScript = (ExtraUIControl)target;
        if (GUILayout.Button("CD10秒"))
        {
            myScript.SetCoolDown(10);
        }
    }
}