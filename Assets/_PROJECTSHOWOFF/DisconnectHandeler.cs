using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using UnityEngine.Events;

public class DisconnectHandeler : MonoBehaviourPunCallbacks
{
    bool activated = false;

    [SerializeField] int minplayerAmount;

    [SerializeField] UnityEvent onLeftRoom = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected) return;
        if (!PhotonNetwork.InRoom) return;

        if (activated)
        {
            if (PhotonNetwork.PlayerList.Length < minplayerAmount)
            {
                PhotonNetwork.LeaveRoom();
            }
            
            return;
        }

        if (PhotonNetwork.PlayerList.Length >= minplayerAmount)
        {
            activated = true;
        }
    }

    private void OnConnectedToServer()
    {
        activated = false;
    }

    public override void OnJoinedRoom()
    {
        activated = false;
        base.OnJoinedRoom();
    }

    public override void OnLeftRoom()
    {
        onLeftRoom?.Invoke();
        base.OnLeftRoom();
    }
}
