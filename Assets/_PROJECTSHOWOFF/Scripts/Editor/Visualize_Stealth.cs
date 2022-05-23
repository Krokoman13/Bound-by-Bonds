using UnityEditor;
using UnityEngine;

namespace playerEditor
{
    [CustomEditor(typeof(PlayerStealth))]
    public class Visualize_Stealth : Editor
    {
        public void OnSceneGUI()
        {
            var t = target as PlayerStealth;
            Transform tr = t.transform;
            Vector3 pos = tr.position;
            Handles.color = Color.yellow;


            Handles.Label(pos, $"Stealth % = {t.currentStealth_perc} \nStealth = {t.currentStealth}");
            Handles.DrawWireDisc(pos, Vector3.up, t.currentStealth, 2);
        }
    }
}