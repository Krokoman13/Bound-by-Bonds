using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using System;

public class PlayerInfoManager : MonoBehaviourPun
{
    [SerializeField] Sibling currentPlayerType;

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        if (PlayerInfo.currentSibling == currentPlayerType) return;
        setPlayerType(currentPlayerType);
    }

    private void Awake()
    {
        SetPlayerType(PlayerInfo.currentSibling);
    }

    private void Update()
    {
        if (PlayerInfo.currentSibling == currentPlayerType) return;
        SetPlayerType(currentPlayerType);
    }

    private void SetPlayerType(Sibling pPlayerType)
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                currentPlayerType = PlayerInfo.currentSibling;
                return;
            }

            switch (pPlayerType)
            {
                case Sibling.Denys:
                    photonView.RPC(nameof(setPlayerType), RpcTarget.OthersBuffered, Sibling.Dana);
                    break;

                case Sibling.Dana:
                    photonView.RPC(nameof(setPlayerType), RpcTarget.OthersBuffered, Sibling.Denys);
                    break;

                case Sibling.UNKNOWN:
                    photonView.RPC(nameof(setPlayerType), RpcTarget.OthersBuffered, Sibling.UNKNOWN);
                    break;
            }
        }

        setPlayerType(pPlayerType);
    }

    [PunRPC]
    private void setPlayerType(Sibling pPlayerType)
    {
        PlayerInfo.currentSibling = pPlayerType;
        currentPlayerType = pPlayerType;
        StartCoroutine(changed());
    }

    IEnumerator changed()
    {
        PlayerInfo.changed = true;
        yield return new WaitForSeconds(Time.fixedDeltaTime * 2);
        PlayerInfo.changed = false;
    }
}
