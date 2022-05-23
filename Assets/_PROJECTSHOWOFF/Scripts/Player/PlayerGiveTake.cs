using System.Collections.Generic;
using TMPro;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(PlayerInteractCheck))]
public class PlayerGiveTake : PlayerComponent
{
    PlayerInteractCheck pInteractCheck = null;
    [SerializeField] TextMeshProUGUI inventoryText = null;

    bool giveTakeInput = false;

    int inv_bandagesAmount = 0;
    int inv_ammo = 0;
    int inv_consumable = 0;


    [Header("Input")]
    [SerializeField] string givetakeInput_name = null;

    [Header("Slider Variables")]
    [SerializeField] PickupSlider pickupSlider = null;
    [SerializeField, Range(0.01f, 2f)] float sliderFillSpeed = .2f;



    void Test_SliderComplete()
    {
        int value = 1;
        if (pInteractCheck.highlightedTarget.interactType == Interactable.InteractionType.Delivering)
        {
            value = -1;
        }
        else
            Destroy(pInteractCheck.highlightedTarget.gameObject);

        switch (pInteractCheck.highlightedTarget.itemType)
        {
            case Interactable.ItemType.Bandages:
                inv_bandagesAmount += value;
                break;
            case Interactable.ItemType.Ammo:
                inv_ammo += value;
                break;
            case Interactable.ItemType.Consumable:
                inv_consumable += value;
                break;
            case Interactable.ItemType.NONE:
                break;
            default:
                break;
        }
        UpdateInventoryText();
        
        
    }


    public override void OnPlayerUpdate()
    {
        if (pInteractCheck.highlightedTarget == null)
        {
            giveTakeInput = false;

        }
        giveTakeInput = Input.GetButton(givetakeInput_name);


        inventoryText.gameObject.SetActive(Input.GetButton("Map"));
    }

    public override void OnPlayerFixedUpdate()
    {
        if (giveTakeInput)
        {
            pickupSlider.AddSliderFill(sliderFillSpeed, true);
        }
        else
        {
            pickupSlider.AddSliderFill(-sliderFillSpeed, true);
        }
    }


    public override void OnPlayerStart(Player pPlayer)
    {
        TryGetComponent<PlayerInteractCheck>(out pInteractCheck);
        if (Debugging())
            return;


        pickupSlider.onSliderComplete += Test_SliderComplete;

        pInteractCheck.onHighlightStart += OnHighlightStart;
        pInteractCheck.onHighlightStop += OnHighlightStop;

        UpdateInventoryText();

    }

    void UpdateInventoryText()
    {
        StringBuilder sb = new StringBuilder("");
        sb.Append($"Bandages: {inv_bandagesAmount}\n");
        sb.Append($"Ammo: {inv_ammo}\n");
        sb.Append($"Consumable: {inv_consumable}");

        if(inventoryText != null)
            inventoryText.text = sb.ToString();
    }


    void OnHighlightStart()
    {
        Interactable.ItemType itemType = pInteractCheck.highlightedTarget.itemType;

        //When collecting, ensure you've got enough items.
        if(pInteractCheck.highlightedTarget.interactType == Interactable.InteractionType.Delivering)
        {
            //Not Enough
            if (itemType == Interactable.ItemType.Ammo && inv_ammo <= 0 ||
                itemType == Interactable.ItemType.Bandages && inv_bandagesAmount <= 0 ||
                itemType == Interactable.ItemType.Consumable && inv_consumable <= 0)
            {
                pickupSlider.SetSlider(pInteractCheck.highlightedTarget);
                pickupSlider.SetNotEnough(pInteractCheck.highlightedTarget);
                pickupSlider.gameObject.SetActive(true);
                pickupSlider.DisableSlider();
                return;
            }
        }
        
        //Enough
        pickupSlider.SetSlider(pInteractCheck.highlightedTarget);
        pickupSlider.gameObject.SetActive(true);
        pickupSlider.EnableSlider();
    }
    void OnHighlightStop()
    {
        pickupSlider.gameObject.SetActive(false);
        pickupSlider.DisableSlider();
    }

    bool Debugging()
    {
        bool bugged = false;
        if (pickupSlider == null)
        {
            Debug.LogError("ERROR, the pickupSlider was not assigned. Deactivating this script.", this);
            this.enabled = false;
        }
        if (pInteractCheck == null)
        {
            Debug.LogError("ERROR, the PlayerInteractCheck component was not found. Deactivating this script", this);
            this.enabled = false;
        }

        return bugged;
    }
}