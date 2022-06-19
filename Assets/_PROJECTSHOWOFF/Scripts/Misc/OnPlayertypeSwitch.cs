using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPlayertypeSwitch : MonoBehaviour
{

    [SerializeField] UnityEvent onDana;
    [SerializeField] UnityEvent onDenys;

    Sibling currentSibling = Sibling.UNKNOWN;

    // Start is called before the first frame update
    void Awake()
    {
        callEvent();
    }

    private void Update()
    {
        if (currentSibling == PlayerInfo.currentSibling) return;
        currentSibling = PlayerInfo.currentSibling;
        callEvent();
    }

    private void callEvent()
    {
        switch (PlayerInfo.currentSibling)
        {
            case Sibling.Dana:
                onDana?.Invoke();
                break;

            case Sibling.Denys:
                onDenys?.Invoke();
                break;
        }
    }
}
