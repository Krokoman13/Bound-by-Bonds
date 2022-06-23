using Cinemachine;
using UnityEngine;

public class PlayerFPSCamHandler : PlayerComponent
{
    //Caching for convenience
    Transform t = null;

    Transform camT = null;
    CinemachineBrain camBrain = null;


    public override void OnPlayerUpdate()
    {
        if (camT == null)
        {
            Debug.LogError("Camera Transform not found. Is there a MainCamera", this);
            return;
        }
        //Player should look in the same direction as the cam but not rotate,so Y axis only.
        t.eulerAngles = new Vector3(t.eulerAngles.x, camT.eulerAngles.y, t.eulerAngles.z);
    }
    public override void OnPlayerStart(Player pPlayer)
    {
        GetAllComponents();

        try
        {
            PauseManager.instance.onPauseGame.AddListener(OnPause);
            PauseManager.instance.onUnpauseGame.AddListener(OnUnpause);
        }
        catch { Debug.LogError("Couldn't find the PauseManager. You can keep playing, though.", this); }

        OnUnpause();
    }


    public void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnPause()
    {
        EnableCursor();
        if (camBrain != null)
            camBrain.enabled = false;
    }
    public void OnUnpause()
    {
        DisableCursor();
        if (camBrain != null)
            camBrain.enabled = true;
    }



    void GetAllComponents()
    {
        t = this.transform;
        camT = Camera.main.transform;
    }
}