using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class MainMenuHandler : MonoBehaviourPunCallbacks
{

    [SerializeField] TMP_InputField hostInputField = null;
    [SerializeField] TMP_InputField joinInputField = null;


    [SerializeField] UnityEvent onCreateRoomSuccesfull = null;
    [SerializeField] UnityEvent onJoinRoomSuccesfull = null;

    [SerializeField] UnityEvent onLobbyFilledEvent = null;

    bool isFull = false;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PhotonNetwork.ConnectUsingSettings();
    }


    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsConnected)
        {            
            return;
        }

        //if (!PhotonNetwork.IsMasterClient)
        //{            
        //    return;
        //}

        if (PhotonNetwork.PlayerList.Length > 1 && !isFull)
        {
            Debug.Log("filled!");
            isFull = true;
            onLobbyFilledEvent?.Invoke();
            return;
        }
    }


    public void CreateRandomRoom()
    {        
        string roomName = GetRoomString(hostInputField.characterLimit);      

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Created room: " + roomName + ". Waiting for another player.");
        hostInputField.text = roomName;
    }


    public void FindRoom()
    {
        Debug.Log("Searching for a Game...");

        if (string.IsNullOrEmpty(joinInputField.text) || joinInputField.text.Length != hostInputField.characterLimit)
        {
            Debug.Log("Warn the player that they've not filled in the field.");
            //PhotonNetwork.JoinRandomRoom();
            return;
        }


        //Try to join a room
        if(PhotonNetwork.JoinRoom(joinInputField.text))
        {
            Debug.Log("Succesfully joining");            
        }
        else
        {
            Debug.Log("Joining Unsuccesful");
        }
    }

    public void StopSearch()
    {        
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
        Debug.Log("Stopped Connecting");
    }


    #region Automatic Photon Code
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
            return;

        onJoinRoomSuccesfull?.Invoke();
        print("Joined Room");
        //PhotonNetwork.AutomaticallySyncScene = true;
        //SceneManager.LoadScene("Lobby");
    }

    public override void OnCreatedRoom()
    {
        print("Created Room");
        onCreateRoomSuccesfull?.Invoke();
        //PhotonNetwork.AutomaticallySyncScene = true;
        //SceneManager.LoadScene("Lobby");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Established conntection to Photon: " + PhotonNetwork.CloudRegion + "Server");
        PhotonNetwork.AutomaticallySyncScene = true;        
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not find Room: " + joinInputField.text);        
        //PhotonNetwork.JoinRandomRoom();
        StopSearch();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Could not find Room");
        StopSearch();
    }
    #endregion

    string GetRoomString(int characterAmount)
    {
        //Can't be 0. Should have a limit somewhere.
        if (characterAmount == 0) characterAmount = 4;

        //Build the room name as string.
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < characterAmount; i++)
            sb.Append(Random.Range(0, 10));
       
        return sb.ToString();
    }

}