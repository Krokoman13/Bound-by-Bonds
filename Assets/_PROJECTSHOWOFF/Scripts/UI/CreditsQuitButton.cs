using UnityEngine.Events;
using UnityEngine;

public class CreditsQuitButton : MonoBehaviour
{
    [SerializeField]string buttonName = "";

    [SerializeField] UnityEvent onButtonClick = null;

    void Update()
    {
        if(Input.GetButton(buttonName))
        {
            onButtonClick?.Invoke();
        }    
    }
}