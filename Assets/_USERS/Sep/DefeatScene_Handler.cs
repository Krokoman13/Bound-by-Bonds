using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class DefeatScene_Handler : MonoBehaviour
{
    public UnityEvent onContinueEvent = null;    

    [SerializeField] TextMeshProUGUI text = null;
    [SerializeField] string continueMessage = "";
    [SerializeField] KeyCode bro = new KeyCode();        

    void Update()
    {    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onContinueEvent?.Invoke();            
        }
    }


    void Start()
    {
        continueMessage = continueMessage.Replace("*", bro.ToString());
        text.text = $"{continueMessage}";

    }
}