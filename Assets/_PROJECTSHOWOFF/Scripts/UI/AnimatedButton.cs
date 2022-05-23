using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(ButtonEventActions))]
[RequireComponent(typeof(ButtonEventActions))]
public class AnimatedButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{

    public enum ButtonState { Idle, Hovering, Pressed }
    [HideInInspector]public ButtonState currentState = ButtonState.Idle;

    ButtonEventActions onHover = null;
    ButtonEventActions onPress = null;


    void Awake()
    {
        ButtonEventActions[] eventActions = GetComponents<ButtonEventActions>();
        if (eventActions.Length < 2)
            Debug.LogWarning("ERROR: This button does not have access to hover and press functions.", this);
        else
        {
            onHover = eventActions[0];
            onPress = eventActions[1];
        }
    }

    /// <summary>
    /// Adds an event to either the Hovering or Press functions' active/enter/exit actions. PLEASE READ PARAMETERS.
    /// </summary>
    /// <param name="eventType">0 = Hovering, --- 1 = Pressing</param>
    /// <param name="actionType">0 = Active (continuous calls when event is true) --- 1 = onEnter (initial call) --- 2 = onExit (once when stopping the action)</param>
    /// <param name="newAction">The method to call when the action has been performed (e.g. a button has been pressed)</param>
    public void AddAction(int eventType, int actionType, UnityAction newAction)
    {
        ButtonEventActions e = onHover;
        if (eventType == 1)
            e = onPress;

        if (actionType == 0)
            e.active.AddListener(newAction);
        else if (actionType == 1)
            e.enter.AddListener(newAction);
        else if (actionType == 2)
            e.exit.AddListener(newAction);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        currentState = ButtonState.Hovering;

        onHover.enter?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {

        if (currentState == ButtonState.Hovering)
            onHover.exit?.Invoke();
        else if (currentState == ButtonState.Pressed)
            onPress.exit?.Invoke();

        currentState = ButtonState.Idle;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        currentState = ButtonState.Pressed;
        onPress.enter?.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        currentState = ButtonState.Hovering;
        onPress.exit?.Invoke();
        onHover.enter?.Invoke();
    }

    void FixedUpdate()
    {
        if (currentState == ButtonState.Hovering)
            onHover.active?.Invoke();
        else if (currentState == ButtonState.Pressed)
            onPress.active?.Invoke();
    }
}