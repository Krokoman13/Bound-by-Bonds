using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class SetupLobbyUI : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI roomIDText;
    [SerializeField] TextMeshProUGUI denysIdentifier;
    [SerializeField] TextMeshProUGUI danaIdentifier;

    [SerializeField] GameObject operatorActive;
    [SerializeField] GameObject startGame;

    void Awake()
    {
        startGame.SetActive(false);

        if (!PhotonNetwork.IsConnected)
        {
            setDenys();
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            setDenys();
            photonView.RPC(nameof(setLobbyName), RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.Name);
            return;
        }

        setDana();
    }

    private void Update()
    {
        if (startGame.activeSelf)
        {
            return;
        }

        if (!PhotonNetwork.IsConnected)
        {
            startGame.SetActive(true);
            return;
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            startGame.SetActive(false);
            return;
        }

        if (PhotonNetwork.PlayerList.Length > 0)
        {
            startGame.SetActive(true);
            return;
        }


        startGame.SetActive(false);
    }

    private void setDenys()
    {
        PlayerInfo.currentSibling = Sibling.Denys;
        denysIdentifier.text = "(You)";
        danaIdentifier.text = "(Other)";
    }

    private void setDana()
    {
        PlayerInfo.currentSibling = Sibling.Dana;
        denysIdentifier.text = "(Other)";
        danaIdentifier.text = "(You)";

        photonView.RPC(nameof(activateOperator), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void activateOperator()
    {
        operatorActive.SetActive(true);
    }

    [PunRPC]
    void setLobbyName(string s)
    {
        roomIDText.text = "Room ID: " + s;
    }
}
