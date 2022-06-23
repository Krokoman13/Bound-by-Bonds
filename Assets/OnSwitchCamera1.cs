using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSwitchCamera : MonoBehaviour
{

    [SerializeField] Camera camera;

    [SerializeField] LayerMask playerSoldierMask;
    [SerializeField] LayerMask playerDroneMask;

    PlayerType currentPlayerType = PlayerType.UNKNOWN;

    // Start is called before the first frame update
    void Awake()
    {
        if (camera == null) camera = GetComponent<Camera>();
        setCulling();
    }

    private void Update()
    {
        if (currentPlayerType == PlayerInfo.playerType) return;
        currentPlayerType = PlayerInfo.playerType;
        setCulling();
    }

    private void setCulling()
    {
        switch (PlayerInfo.playerType)
        {
            case PlayerType.Soldier:
                camera.cullingMask = playerSoldierMask;
                break;

            case PlayerType.Operator:
                camera.cullingMask = playerDroneMask;
                break;
        }
    }
}
