using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerFPSGroundCheck)), CanEditMultipleObjects]
public class Visualize_GroundCheck : Editor
{
    PlayerFPSGroundCheck t = null;
    public void OnSceneGUI()
    {
        if (t == null)
            t = target as PlayerFPSGroundCheck;

        Transform tr = t.transform;
        float lineSize = 5;
        float lineThickness = 1;

        //Debug            
        Handles.color = Color.green;
        Handles.DrawLine(tr.position, tr.position + (t.currentSlopeNormal * lineSize), lineThickness);
        Handles.color = Color.red;
        Handles.DrawLine(tr.position, tr.position + (t.slopeRight * lineSize), lineThickness);
        Handles.color = Color.blue;
        Handles.DrawLine(tr.position, tr.position + (t.currentSlopeDir * lineSize), lineThickness);

        
        Transform check = t.floorCheckBox;
        Vector3 low = (tr.up * check.localScale.y);
        Handles.color = Color.red;
        for(int i = 0; i < t.floorcheck_bounds.Length; i++)
        {
            int next = i + 1;
            if (next >= t.floorcheck_bounds.Length)
                next = 0;

            //Horizontal lines -- topsides
            Handles.DrawLine(t.floorcheck_bounds[i], t.floorcheck_bounds[next], lineThickness);
            //Horizontal lines -- bottomsides
            Handles.DrawLine(t.floorcheck_bounds[i] - low, t.floorcheck_bounds[next] - low, lineThickness);
            //Vertical lines -- corners
            Handles.DrawLine(t.floorcheck_bounds[i], t.floorcheck_bounds[i] - low, lineThickness);
        }
    }
}