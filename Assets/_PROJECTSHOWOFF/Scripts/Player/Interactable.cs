using System;
using UnityEngine;

using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public enum InteractionType {Collecting, Delivering, NONE}   //can this object be taken or does it receive an item?
    public InteractionType interactType = InteractionType.Collecting;

    public enum ItemType {Bandages, Ammo, Consumable, NONE}
    public ItemType itemType = ItemType.NONE;
    public string itemName = "NewItem";

    //PLAYER COMPONENTS
    public delegate void InteractableDelegate(Interactable player);
    public InteractableDelegate onInteractableStart = null;
    public Action onInteractableUpdate = null;
    public Action onInteractableFixedUpdate = null;

    //Interacting with player
    //Highlight
    public Action onInteractableHighlight_Start = null;
    public Action onInteractableHighlight_Stop = null;

    //PAUSE
    public Action onPauseAction = null;
    public Action onUnpauseAction = null;
    bool isPaused = false;


    void Start()
    {
        PauseManager pause = GetPauseManager();

        pause.onPauseGame?.AddListener(OnPause);
        pause.onUnpauseGame?.AddListener(OnUnpause);


        onInteractableStart?.Invoke(this);
    }

    void Update()
    {
        if (isPaused)
            return;

        onInteractableUpdate?.Invoke();
    }

    void FixedUpdate()
    {
        if (isPaused)
            return;

        onInteractableFixedUpdate?.Invoke();
    }


    public void OnHighlightStart()
    {
        onInteractableHighlight_Start?.Invoke();
    }
    public void OnHighlightStop()
    {
        onInteractableHighlight_Stop?.Invoke();
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
        PauseManager pause = PauseManager.instance;
        if (pause == null)
        {
            Debug.LogError("Error, pauseManager could not be found.", this);
        }
        return pause;
    }
}