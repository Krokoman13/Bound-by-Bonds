using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SetOwner : MonoBehaviourPun
{
    [SerializeField] Sibling playerType;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Destroy(this);
        }
    }

    private void FixOwnership()
    {
        bool state = PlayerInfo.currentSibling == playerType;

        if (state)
        {
            photonView.RequestOwnership();
        }
    }

    private void FixedUpdate()
    {
        if (!PlayerInfo.changed) return;
        FixOwnership();
    }
}
