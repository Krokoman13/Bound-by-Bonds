using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSwitchCamera : MonoBehaviour
{

    [SerializeField] Camera cam;

    [SerializeField] LayerMask danaMask;
    [SerializeField] LayerMask denysMask;

    Sibling currentSibling = Sibling.UNKNOWN;

    // Start is called before the first frame update
    void Awake()
    {
        if (cam == null) cam = GetComponent<Camera>();
        setCulling();
    }

    private void Update()
    {
        if (currentSibling == PlayerInfo.currentSibling) return;
        currentSibling = PlayerInfo.currentSibling;
        setCulling();
    }

    private void setCulling()
    {
        switch (PlayerInfo.currentSibling)
        {
            case Sibling.Dana:
                cam.cullingMask = danaMask;
                break;

            case Sibling.Denys:
                cam.cullingMask = denysMask;
                break;
        }
    }
}
