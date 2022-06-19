using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//[CustomEditor(typeof(Interactable))]
public class Editor_Interactable : Editor
{
    Interactable t = null;

    SerializedProperty sp_itemType = null;
    SerializedProperty sp_itemName = null;

    SerializedProperty sp_useAmount = null;

    SerializedProperty sp_interactType = null;
    SerializedProperty sp_ontakeEvent = null;
    SerializedProperty sp_onGiveEvent = null;
    SerializedProperty sp_onInteractEvent = null;


    //private void OnEnable()
    //{
    //    sp_itemType = serializedObject.FindProperty("itemType");
    //    sp_itemName = serializedObject.FindProperty("itemName");

    //    sp_useAmount = serializedObject.FindProperty("useAmount");

    //    sp_interactType = serializedObject.FindProperty("interactType");
    //    sp_ontakeEvent = serializedObject.FindProperty("onGiveEvent");
    //    sp_onGiveEvent = serializedObject.FindProperty("onTakeEvent");
    //    sp_onInteractEvent = serializedObject.FindProperty("onInteractEvent");

    //}

    //public override void OnInspectorGUI()
    //{

    //    // fetch current values from the real instance into the serialized "clone"
    //    serializedObject.Update();

    //    EditorGUILayout.PropertyField(sp_interactType);
    //    EditorGUILayout.PropertyField(sp_itemType);
    //    EditorGUILayout.PropertyField(sp_itemName);
    //    EditorGUILayout.PropertyField(sp_useAmount);

    //    //GIVE
    //    if (sp_interactType.intValue == 0)
    //        EditorGUILayout.PropertyField(sp_onGiveEvent);
    //    else if (sp_interactType.intValue == 1)
    //        EditorGUILayout.PropertyField(sp_ontakeEvent);
    //    else if (sp_interactType.intValue == 2)
    //        EditorGUILayout.PropertyField(sp_onInteractEvent);

    //    serializedObject.ApplyModifiedProperties();
        
    //}
}