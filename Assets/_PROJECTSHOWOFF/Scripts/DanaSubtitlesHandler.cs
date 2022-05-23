using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanaSubtitlesHandler : SubtitlesHandler
{
    public static DanaSubtitlesHandler instance;

    private void Start()
    {
        if (instance != null) Debug.LogWarning(
                "Multiple instances of SoldierSubtitlesHandler exist:" + instance.gameObject.name +" and " + gameObject.name + 
                ", this should only be one");

        instance = this;
    }
}
