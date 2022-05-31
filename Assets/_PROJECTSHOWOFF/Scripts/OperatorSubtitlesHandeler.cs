using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorSubtitlesHandeler : SubtitlesHandler
{
    public static OperatorSubtitlesHandeler instance;

    private void Start()
    {
        if (instance != null) Debug.LogWarning(
                "Multiple instances of OperatorSubtitlesHandeler exist:" + instance.gameObject.name + " and " + gameObject.name +
                ", this should only be one");

        instance = this;
    }
}
