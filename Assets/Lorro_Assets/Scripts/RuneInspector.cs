using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Rune))]
public class RuneInspector : Editor
{
    float scaleFactor = 1.0f;
    Vector3 rotation = new Vector3(0, 0, 0);
    Vector3 translation = new Vector3(0, 0, 0);

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Rune rune = (Rune)target;

        //Scale
        scaleFactor = EditorGUILayout.FloatField("Scale Factor", scaleFactor);

        if (GUILayout.Button("Scale"))
        {
            rune.Scale(scaleFactor);
        }

        //Rotate
        rotation.x = EditorGUILayout.FloatField("Rotation X°", rotation.x);
        rotation.y = EditorGUILayout.FloatField("Rotation Y°", rotation.y);
        rotation.z = EditorGUILayout.FloatField("Rotation Z°", rotation.z);

        if (GUILayout.Button("Rotate"))
        {
            rune.Rotate(rotation);
        }

        //Translate
        translation.x = EditorGUILayout.FloatField("Translation X", translation.x);
        translation.y = EditorGUILayout.FloatField("Translation Y", translation.y);
        translation.z = EditorGUILayout.FloatField("Translation Z", translation.z);

        if (GUILayout.Button("Translate"))
        {
            rune.Translate(translation);
        }

        //Split (=> higher sample rate)
        if (GUILayout.Button("Split"))
        {
            rune.Split();
        }

        //Combine (=> lower sample rate)
        if (GUILayout.Button("Combine"))
        {
            rune.Combine();
        }

        if (GUILayout.Button("Center on Transform"))
        {
            rune.Center();
        }
    }
}
