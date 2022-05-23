using TMPro;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class PickupSlider : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Image slider = null;
    [SerializeField] Image interactionImage = null;
    [SerializeField] TextMeshProUGUI text = null;


    [Header("Sprites")]
    [SerializeField] Sprite sprite_take = null;
    [SerializeField] Sprite sprite_give = null;
    [SerializeField] Sprite sprite_notEnough = null;

    [Header("Descriptions")]
    [SerializeField] string description_take = "Hold [E] to take";
    [SerializeField] string description_give = "Hold [E] to give";
    [SerializeField] string description_notEnough = "You don't have this item";


    Interactable.InteractionType currentType = Interactable.InteractionType.NONE;

    bool interactable = true;
    public System.Action onSliderComplete = null;


    public void SetText(string s)
    {
        text.text = s;
    }

    public void SetSlider(Interactable interactable)
    {
        Interactable.InteractionType newType = interactable.interactType;

        //New state = NONE ---> means it should disable the slider.
        if (newType == Interactable.InteractionType.NONE)
            DisableSlider();
        //Current state = NONE ---> means it was NONE but isn't now (otherwise it wouldn't get past the other if-statements)
        else if (currentType == Interactable.InteractionType.NONE)
            EnableSlider();

        //Set the new values
        currentType = newType;
        interactionImage.sprite = GetSprite(newType);
        text.text = GetDescription(interactable);
    }

    public void SetNotEnough(Interactable interactable)
    {
        text.text = description_notEnough + $" ({interactable.itemName})";
        interactionImage.sprite = sprite_notEnough;
    }


    public void AddSliderFill(float increment, bool fixedDeltaTime)
    {
        if (!interactable)
            return;

        //Determine time function
        float time = Time.fixedDeltaTime;
        if (!fixedDeltaTime)
            time = Time.deltaTime;

        //Adjust Slider Fill
        slider.fillAmount += increment * time;
        slider.fillAmount = Mathf.Clamp01(slider.fillAmount);

        //Check if passed
        if(slider.fillAmount >= 1)
        {
            DisableSlider();
            gameObject.SetActive(false);
            onSliderComplete?.Invoke();
        }
    }    

    public void ResetSliderFill()
    {
        slider.fillAmount = 0;
    }


    public void DisableSlider()
    {        
        interactable = false;
        ResetSliderFill();
    }
    public void EnableSlider()
    {
        interactable = true;
        ResetSliderFill();        
    }


    Sprite GetSprite(Interactable.InteractionType newType)
    {
        //Get the sprite associated with this state
        Sprite newSprite = sprite_give;
        if (newType == Interactable.InteractionType.Collecting)
            newSprite = sprite_take;
        
        return newSprite;
    }
    string GetDescription(Interactable interactable)
    {
        Interactable.InteractionType newType = interactable.interactType;

        //Get the description associated with this state
        string newString = description_give;
        if (newType == Interactable.InteractionType.Collecting)
            newString = description_take;
        
        StringBuilder sb = new StringBuilder(newString + $" {interactable.itemName}");
        return sb.ToString();
    }


    void Awake()
    {
        Debugging();
        DisableSlider();
    }

    void Debugging()
    {
        if (slider == null)
        {
            Debug.LogError("Slider not assigned, can't setup PickupSlider component.", this);
            this.enabled = false;
        }

        if (sprite_give == null || sprite_take == null)
        {
            Debug.LogError("Slider Give or Take sprites aren't assigned.", this);
        }
    }
}