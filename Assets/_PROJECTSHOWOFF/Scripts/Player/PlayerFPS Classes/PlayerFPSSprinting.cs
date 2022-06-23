using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(PlayerFPSMovement))]
[RequireComponent(typeof(PlayerFPSGroundCheck))]
[RequireComponent(typeof(PlayerStamina))]
public class PlayerFPSSprinting : PlayerComponent
{
    PlayerFPSMovement movement = null;
    PlayerFPSGroundCheck groundcheck = null;
    PlayerStamina stamina = null;

    //Sprinting State
    bool sprintInput = false;
    [HideInInspector] public bool isSprinting = false;
    bool isSprintingAllowed = true;

    [SerializeField] string sprintInputName = "Fire3";

    [Header("Speed variables")]
    [SerializeField] float acc_sprint = 10;
    [SerializeField] float acc_airSprint = 10;
    float acc_normalSpeed = 0;
    float acc_normalAirSpeed = 0;

    [Header("Stamina depletion/recovering")]
    [SerializeField] float staminaDepleteSpeed = 1;
    [SerializeField] float staminaRecoverSpeed = 1.2f;

    [Header("Sprint Cooldown Values")]
    bool isSprintCooldownActive = false;
    [SerializeField] float staminaCooldown_SpamPrevent_sec = 1;
    [SerializeField] float staminaCooldown_EmptyPunish_sec = 4;
    float currentSprintCooldown = 0;

    [Header("PostProcessing Volume")]
    [SerializeField] Volume volume = null;

    [SerializeField]UnityEvent onSprint = null;
    [SerializeField]UnityEvent onEndSprint = null;


    public override void OnPlayerFixedUpdate()
    {
        SprintCooldownTimer();
        CheckStamina();

        //Don't sprint under these conditions
        if (movement.inputs.y <= 0 || groundcheck.tooSteep || isSprintCooldownActive || !isSprintingAllowed)
        {
            if (isSprinting)
            {
                OnSprintStop();
                OnSprintCooldown(staminaCooldown_SpamPrevent_sec);
            }
            return;
        }
        
        if (sprintInput && !isSprinting)
        {            
            OnSprint();
        }
        else if (!sprintInput && isSprinting)
        {
            OnSprintStop();
            OnSprintCooldown(staminaCooldown_SpamPrevent_sec);
        }
    }

    void CheckStamina()
    {
        if (isSprinting)
        {
            stamina.DepleteStamina(staminaDepleteSpeed, true);
            if (stamina.currentStamina <= 0)
            {
                OnSprintStop();
                OnSprintCooldown(staminaCooldown_EmptyPunish_sec);
            }
        }
        else
        {
            stamina.RecoverStamina(staminaRecoverSpeed, true);
        }
    }



    void OnSprintCooldown(float duration)
    {
        sprintInput = false;

        currentSprintCooldown = duration;
        isSprintCooldownActive = true;
    }

    void SprintCooldownTimer()
    {
        if (!isSprintCooldownActive)
            return;

        currentSprintCooldown -= Time.fixedDeltaTime;
        if (currentSprintCooldown <= 0)
        {
            isSprintCooldownActive = false;
        }
    }


    void OnSprint()
    {
        if (movement.MovementIsOverridenByOther(this))
            return;

        isSprinting = true;
        movement.acc_speed = acc_sprint;
        movement.acc_airspeed = acc_airSprint;

        movement.NotifyStartOverride(this);
        onSprint?.Invoke();
    }

    void OnSprintStop()
    {
        isSprinting = false;
        movement.acc_speed = acc_normalSpeed;
        movement.acc_airspeed = acc_normalAirSpeed;

        movement.NotifyStopOverride(this);
        onEndSprint?.Invoke();
    }



    void OnMovementOverrideStart(PlayerComponent origin)
    {
        if (origin is PlayerFPSSprinting)
            return;

        if (isSprinting)
        {            
            OnSprintStop();
            OnSprintCooldown(staminaCooldown_SpamPrevent_sec);
            isSprintingAllowed = false;
        }
    }
    void OnMovementOverrideStop(PlayerComponent origin)
    {
        if (origin is PlayerFPSSprinting)
            return;

        isSprintingAllowed = true;
    }

    public override void OnPlayerStart(Player pPlayer)
    {
        TryGetComponent<PlayerFPSMovement>(out movement);
        TryGetComponent<PlayerFPSGroundCheck>(out groundcheck);
        TryGetComponent<PlayerStamina>(out stamina);

        if (volume == null) FindObjectOfType<Volume>();

        acc_normalSpeed = movement.acc_speed;
        acc_normalAirSpeed = movement.acc_airspeed;

        movement.onMovementOverrideStart += OnMovementOverrideStart;
        movement.onMovementOverrideStop += OnMovementOverrideStop;
    }

    public override void OnPlayerUpdate()
    {
        sprintInput = Input.GetButton(sprintInputName);
    }

}