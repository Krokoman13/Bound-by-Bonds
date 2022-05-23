using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    //PLAYER COMPONENTS
    public delegate void PlayerDelegate(Player player);
    public PlayerDelegate onPlayerStart = null;
    public Action onPlayerUpdate = null;
    public Action onPlayerFixedUpdate = null;
    public Action onPlayerDeath = null;

    //PAUSE
    public Action onPauseAction = null;
    public Action onUnpauseAction = null;
    bool isPaused = false;


    void Start()
    {
        PauseManager pause = GetPauseManager();
        if (pause != null)
        {
            pause.onPauseGame?.AddListener(OnPause);
            pause.onUnpauseGame?.AddListener(OnUnpause);
        }


        onPlayerStart?.Invoke(this);
    }

    void Update()
    {
        if (isPaused)
            return;

        onPlayerUpdate?.Invoke();
    }

    void FixedUpdate()
    {
        if (isPaused)
            return;

        onPlayerFixedUpdate?.Invoke();
    }

    public void PlayerDead()
    {
        onPlayerDeath?.Invoke();
    }

    public void OnPause()
    {
        isPaused = true;
        onPauseAction?.Invoke();
    }
    public void OnUnpause()
    {
        isPaused = false;
        onUnpauseAction?.Invoke();
    }

    PauseManager GetPauseManager()
    {
        try
        {
            PauseManager pause = PauseManager.instance;
            return pause;
        }
        catch
        {
            Debug.LogError("Error, pauseManager could not be found.", this);
        }
        return null;
    }
}