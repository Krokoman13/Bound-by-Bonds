using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Subtitle
{
    [HideInInspector] public float appearTime = 1;
    [HideInInspector] public string speaker = "Developer";
    [HideInInspector] public string text = "This is a test message.";
    [HideInInspector] public string audioKey = "Voice_Line_[TYPE]_[Index]";

    public string getText
    {
        get
        {
            StringBuilder sb = new StringBuilder($"[{speaker}]: {text}");
            return sb.ToString();
        }
    }
}