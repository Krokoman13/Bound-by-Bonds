using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlyEnableIfHost : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsConnected) return;
        gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }
}
