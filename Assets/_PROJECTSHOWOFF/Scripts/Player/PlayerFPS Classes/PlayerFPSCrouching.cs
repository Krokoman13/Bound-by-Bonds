using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerFPSMovement))]
[RequireComponent(typeof(PlayerFPSGroundCheck))]
public class PlayerFPSCrouching : PlayerComponent
{
    PlayerFPSMovement movement = null;
    PlayerFPSGroundCheck groundcheck = null;

    [Header("Player Colliders")]
    [SerializeField] Collider crouchCol = null;
    [SerializeField] CapsuleCollider col = null;

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera playerFPS_vCam = null;
    [SerializeField] CinemachineVirtualCamera playerFPS_CrouchVCam = null;
    CinemachinePOV defaultPov = null;
    CinemachinePOV crouchPov = null;
    
    [Space(8)]
    [SerializeField] string crouchInputName = "Fire1";
    [Header("Crouching Variables")]
    [SerializeField] float acc_crouch = 10;
    float acc_normalSpeed = 0;
    [SerializeField] LayerMask ceilingCheckMask = ~3;

    //Current state
    bool crouchInput = false;
    bool crouchingAllowed = true;
    bool uncrouchingAllowed = false;
    [HideInInspector] public bool isCurrentlyCrouching = false;

    int crouchCeilingCheckInterval = 10;

    [SerializeField] UnityEvent onCrouch = null;
    [SerializeField] UnityEvent onUncrouch = null;


    void Inputs()
    {
        //Cache inputs in update method
        crouchInput = Input.GetButton(crouchInputName);
    }

    public override void OnPlayerFixedUpdate()
    {
        //You may not crouch while airborne or when something else is holding it back...
        if (!groundcheck.isGrounded || !crouchingAllowed)
        {
            if (isCurrentlyCrouching) OnEndCrouch();
            return;
        }

        //Initial Crouch (once) -- must not be airborne
        if (crouchInput && !isCurrentlyCrouching)
            OnStartCrouch();
        //Initial UnCrouch (once)
        else if (!crouchInput && isCurrentlyCrouching)
            OnEndCrouch();
    }


    void OnStartCrouch()
    {
        if (movement.MovementIsOverridenByOther(this))
            return;
        //Virtual Camera references required for the next part
        //if (!CheckVirtualCam(playerFPS_vCam) || !CheckVirtualCam(playerFPS_CrouchVCam)) return;

        //update state
        isCurrentlyCrouching = true;
        movement.acc_speed = acc_crouch;        //Alter movement speed        
        SwitchColliders(crouchCol, col);

        //Instantly synchronize the new VCam and switch to it
        SyncCameraAxis(crouchPov, defaultPov);
        SwitchPriority(playerFPS_vCam, playerFPS_CrouchVCam);

        movement.NotifyStartOverride(this);
        onCrouch?.Invoke();
    }

    void OnEndCrouch()
    {
        //Virtual Camera references required for the next part
        //if (!CheckVirtualCam(playerFPS_vCam) || !CheckVirtualCam(playerFPS_CrouchVCam)) return;

        //Cannot uncrouch under low ceilings. Prevents clipping through the floor/ceiling
        uncrouchingAllowed = CanUncrouch();
        if (!uncrouchingAllowed) return;

        //Update state
        isCurrentlyCrouching = false;
        movement.acc_speed = acc_normalSpeed;   //Revert movement speed        
        SwitchColliders(col, crouchCol);

        //Instantly synchronize the new VCam and switch to it
        SyncCameraAxis(defaultPov, crouchPov);
        SwitchPriority(playerFPS_CrouchVCam, playerFPS_vCam);

        movement.NotifyStopOverride(this);
        onUncrouch?.Invoke();
    }

    bool CanUncrouch()
    {
        //Draw a line to check if there's a low ceiling that prevents us from uncrouching.
        Vector3 pos1 = crouchCol.bounds.center;
        Vector3 pos2 = col.bounds.center + (transform.up * (col.height / 2));
        Physics.Linecast(pos1, pos2, out RaycastHit hit, ceilingCheckMask, QueryTriggerInteraction.Ignore);
        //Debug.DrawLine(pos1, pos2, Color.magenta);        

        //Return true if no ceiling has been hit. if it has, then return false.
        return hit.collider == null;
    }


    #region convenience methods
    void SwitchColliders(Collider newCol, Collider oldCol)
    {
        oldCol.gameObject.SetActive(false);
        newCol.gameObject.SetActive(true);
    }
    void SyncCameraAxis(CinemachinePOV newPov, CinemachinePOV oldPov)
    {
        newPov.m_HorizontalAxis = oldPov.m_HorizontalAxis;
        newPov.m_VerticalAxis = oldPov.m_VerticalAxis;
    }
    void SwitchPriority(CinemachineVirtualCamera cam1, CinemachineVirtualCamera cam2)
    {
        int priority1 = cam1.Priority,
            priority2 = cam2.Priority;

        cam1.Priority = priority2;
        cam2.Priority = priority1;
    }
    #endregion



    //NOTE: Player can override into sprinting whilst under low ceiling now. Fix with continuous raycast and a sort-of priority system.






    void OnMovementOverrideStart(PlayerComponent origin)
    {   
        //If another component starts overriding movement, stop crouching (since latest goes first)
        //example: whilst crouching, you start sprinting. The latter takes priority so crouching gets cancelled.

        if (origin is PlayerFPSCrouching) return;
        
        //Cancel crouch and don't allow for it until the override is over.
        if (isCurrentlyCrouching)
        {
            OnEndCrouch();
            crouchingAllowed = false;
        }
    }

    void OnMovementOverrideStop(PlayerComponent origin)
    {
        //The other override is over, so we can crouch now.
        if (origin is PlayerFPSCrouching) return;

        crouchingAllowed = true;
    }



    public override void OnPlayerStart(Player pPlayer)
    {
        Debugging();

        defaultPov = playerFPS_vCam.GetCinemachineComponent<CinemachinePOV>();
        crouchPov = playerFPS_CrouchVCam.GetCinemachineComponent<CinemachinePOV>();
        TryGetComponent<PlayerFPSMovement>(out movement);
        TryGetComponent<PlayerFPSGroundCheck>(out groundcheck);

        //Cache default speed for later use.
        acc_normalSpeed = movement.acc_speed;

        movement.onMovementOverrideStart += OnMovementOverrideStart;
        movement.onMovementOverrideStop += OnMovementOverrideStop;
    }

    public override void OnPlayerUpdate()
    {
        Inputs();

        if(isCurrentlyCrouching)
        {
            if (Time.frameCount % crouchCeilingCheckInterval != 0)
                return;

            uncrouchingAllowed = CanUncrouch();
            if(uncrouchingAllowed)
            {
                movement.NotifyStartOverride(this, true);
            }
        }
    }

    void Debugging()
    {
        //Check Colliders
        if (col == null || crouchCol == null)
        {
            Debug.LogError("Player colliders not assigned. Turning off PlayerFPSCrouching Component.", this);
            this.enabled = false;
        }
        //Check Virtual Cameras
        if (!CheckVirtualCam(playerFPS_vCam))
            return;
    }

    bool CheckVirtualCam(CinemachineVirtualCamera vcam)
    {
        if (vcam == null)
        {
            Debug.LogError("Default and/or Crouching Virtual Camera not assigned to PlayerFPSCrouching class.\n" +
                "Turning off PlayerFPSCrouching Component.", this);
            this.enabled = false;
            return false;
        }
        return true;
    }

}