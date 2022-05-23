using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using Photon.Pun;

class SceneSwitcher : MonoBehaviour
{
    static public void SwitchToSceneString(string scene)
    {
        Debug.Log("Loading scene: " + scene);

        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(scene);
            return;
        }

        SceneManager.LoadScene(scene);
    }

    static public void SwitchToSceneInt(int sceneID)
    {

        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneID);
            return;
        }

        SceneManager.LoadScene(sceneID);
    }
}
