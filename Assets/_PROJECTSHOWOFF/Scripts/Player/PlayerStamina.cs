using UnityEngine;

public class PlayerStamina : PlayerComponent
{
    [Header("Stamina Variables")]
    [HideInInspector] public float maxStamina = 20;
    public float currentStamina = 20;
    [HideInInspector] public float staminaPercent = 20;    



    public override void OnPlayerStart(Player pPlayer)
    {
        maxStamina = currentStamina;
        UpdateStaminaPercent();
    }




    public void RecoverStamina(float recover, bool fixedDeltaTime = true)
    {
        recover = Mathf.Max(0, recover);

        //Recover Stamina
        float dTime = Time.deltaTime;
        if(fixedDeltaTime)
            dTime = Time.fixedDeltaTime;

        currentStamina += recover * dTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina); //can't exceed max
        UpdateStaminaPercent();
    }
    public void DepleteStamina(float depletion, bool fixedDeltaTime = true)
    {
        depletion = Mathf.Max(0, depletion);    //depletion must be positive value

        //Recover Stamina
        float dTime = Time.deltaTime;
        if (fixedDeltaTime)
            dTime = Time.fixedDeltaTime;

        currentStamina -= depletion * dTime;
        currentStamina = Mathf.Max(currentStamina, 0);  //can't be lower than 0
        UpdateStaminaPercent();
    }


    void UpdateStaminaPercent()
    {
        staminaPercent = (currentStamina / maxStamina);        
    }


    private void OnValidate()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, currentStamina);
    }
}