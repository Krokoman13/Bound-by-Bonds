using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] float percentAmount;

    public void SetFillPercentage(float amount)
    { 
        amount = Mathf.Clamp(amount, 0, 1);

        fill.fillAmount = amount;
        if (text != null) text.text = (amount * 100).ToString("0") + '%';
    }

    private void OnValidate()
    {
        percentAmount = Mathf.Clamp(percentAmount, 0, 1);
        SetFillPercentage(percentAmount);
    }
}
