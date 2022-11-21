using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using Photon.Pun;

class SceneSwitcher : MonoBehaviour
{
    [SerializeField] bool multiplayer = true;

    public void SwitchToSceneString(string scene)
    {
        Debug.Log("Loading scene: " + scene);

        if (multiplayer && PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(scene);
            return;
        }

        SceneManager.LoadScene(scene);
    }

    public void SwitchToSceneInt(int sceneID)
    {

        if (multiplayer && PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneID);
            return;
        }

        SceneManager.LoadScene(sceneID);
    }

    static public void QuitGame()
    {
        Application.Quit();
    }

}
