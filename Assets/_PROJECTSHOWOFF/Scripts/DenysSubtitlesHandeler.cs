using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenysSubtitlesHandeler : SubtitlesHandler
{
    public static DenysSubtitlesHandeler instance;

    private void Start()
    {
        if (instance != null) Debug.LogWarning(
                "Multiple instances of OperatorSubtitlesHandeler exist:" + instance.gameObject.name + " and " + gameObject.name +
                ", this should only be one");

        instance = this;
    }
}
