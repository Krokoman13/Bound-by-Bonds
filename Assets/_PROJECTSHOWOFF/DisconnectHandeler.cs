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

    static DisconnectHandeler onlyOne;

    private void Awake()
    {
        if (onlyOne != null && onlyOne != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        onlyOne = this;
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
                PhotonNetwork.Disconnect();
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
        //gameObject.SetActive(false);
    }
}
