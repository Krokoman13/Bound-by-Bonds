using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TriggerSubtitle))]
public class Editor_TriggerSubtitle : Editor
{
    TriggerSubtitle t = null;

    public override void OnInspectorGUI()
    {
        //{
        //    if (GUILayout.Button("Add new Point"))
        //    {
        //        if (t == null)
        //            t = target as MortarStrikeSystem;
        //        t.AddNewPoint();
        //    }
        //    if (GUILayout.Button("Add new Point at my position"))
        //    {
        //        if (t == null)
        //            t = target as MortarStrikeSystem;

        //        Vector3 sceneCamPos = SceneView.lastActiveSceneView.camera.transform.position;
        //        t.AddNewPoint(sceneCamPos);
        //    }

        base.OnInspectorGUI();

        t = target as TriggerSubtitle;

        if (t.fmodAudioSource == null)
            EditorGUILayout.HelpBox("ERROR: FMOD-AUDIOSOURCE NOT ASSIGNED.", MessageType.Error);
    }
}