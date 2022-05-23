using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance = null;

    public bool gameIsPaused = false;
    [SerializeField] GameObject pauseMenu;

    public UnityEvent onPauseGame = null;
    public UnityEvent onUnpauseGame = null;

    [SerializeField] string pauseButton_name = "Cancel";


    /// <summary>
    /// Pause or unpause the game, pass true if it should call the onPause event (tip: make it open the pause menu)
    /// </summary>
    /// <param name="openPauseMenu"></param>
    public void DoPause(bool openPauseMenu)
    {
        //Game is already paused?
        if (gameIsPaused)
            return;

        //Pause the game by stopping timescale. pause non time-based scripts here as well.
        gameIsPaused = true;
        //Time.timeScale = 0;


        //Invoke when pausing.
        onPauseGame?.Invoke();
        pauseMenu?.SetActive(openPauseMenu);
    }

    public void DoUnpause()
    {
        //Game is already unpaused?
        if (!gameIsPaused)
            return;

        //Resume time
        gameIsPaused = false;
        //Time.timeScale = 1;
        onUnpauseGame?.Invoke();
        pauseMenu?.SetActive(false);
    }


    void PauseInput()
    {
        //Don't use the input when paused.
        if (gameIsPaused)
            return;

        if (Input.GetButtonDown(pauseButton_name))
        {
            DoPause(true);
        }
    }
    

    public void Toggle_PauseMenu()
    {
        if (gameIsPaused)
            DoUnpause();
        else
            DoPause(true);
    }    


    void Update()
    {
        PauseInput();
    }

    void Awake()
    {
        instance = this;
        DoUnpause();
    }

    public static PauseManager GetPauseManager()
    {
        try
        {
            PauseManager pause = instance;
            return pause;
        }
        catch
        {
            Debug.LogError("Error, pauseManager could not be found.");
        }
        return null;
    }
}