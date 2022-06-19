using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerFPSGroundCheck))]
public class PlayerFPSMovement : PlayerComponent
{
    Rigidbody rb = null;
    PlayerFPSGroundCheck groundCheck = null;

    [HideInInspector] public Vector2 inputs = new Vector2();
    [HideInInspector] public bool isMoving = false;

    [Header("Movement Variables")]
    [Tooltip("The speed at which the player will move when grounded")]
    public float acc_speed = 30f;
    [Tooltip("The player's movespeed when aerial.")]
    public float acc_airspeed = 15f;

    [SerializeField] float slopeFallSpeed = 5f;
    [SerializeField] float fallspeed = 15f;

    [SerializeField] UnityEvent onStartMove = null;
    [SerializeField] UnityEvent onStopMove = null;

    public delegate void MovementOverrideMethod(PlayerComponent origin);
    public MovementOverrideMethod onMovementOverrideStart = null;
    public MovementOverrideMethod onMovementOverrideStop = null;
    public MovementOverrideMethod onMovementOverrideReplaced = null;


    Transform slopeT = null;        //Used for converting movement to 'slope-space'

    void DoMove()
    {
        //Save velocity in a new vec2 to gain more control before applying the changes.        
        Vector3 velo = rb.velocity;
        UpdateSlopeTransform();


        //---Player-Space-Changes---
        velo = slopeT.InverseTransformVector(velo);     //Convert velocity from world-space to 'player-space'.


        velo += Acceleration();                         //Apply acceleration based on Player Input
        velo += FallSpeed();                            //Apply additional fall-speed

        velo = slopeT.TransformVector(velo);            //Convert the velocity back to world-space
        //--------------------------      


        SlopeCheck(ref velo);                           //apply slope-based changes to Velocity [note: ref parameter]                
        rb.velocity = velo;                             //Apply final velocity to rigidbody
    }

    Vector3 Acceleration()
    {
        if (inputs != Vector2.zero) isMoving = true;
        else isMoving = false;

        //Get grounded or aerial accelleration.
        float acc = acc_speed;
        if (!groundCheck.isGrounded) acc = acc_airspeed;

        //Move based on input and acceleration
        Vector3 result = slopeT.forward * inputs.y * acc * Time.fixedDeltaTime;
        result += slopeT.right * inputs.x * acc * Time.fixedDeltaTime;
        //result.y = 0;   

        //Return the acceleration in player-space
        return transform.TransformVector(result);
    }

    Vector3 FallSpeed()
    {
        //Don't apply fallspeed if we're not midair                
        if (groundCheck != null && (groundCheck.isGrounded || rb.velocity.y > 0))
            return Vector3.zero;

        //Lower velocity based on fallspeed. (note that this is negative).        
        float acc_Y = -(fallspeed * Time.fixedDeltaTime);
        return transform.up * acc_Y;
    }

    void SlopeCheck(ref Vector3 velo)
    {
        //float maxAngle = groundCheck.maxSlopeAngle;
        float angle = groundCheck.currentSlopeAngle;
        Vector3 dir = groundCheck.currentSlopeDir;

        //Fall down steep slopes
        if (groundCheck.tooSteep)
        {
            rb.useGravity = true;       //angle is too steep: force player to airborne state
            velo += dir * angle * slopeFallSpeed * Time.fixedDeltaTime;     //Force player down the slope a bit.
            return;
        }

        //velo.y = 0;
    }

    void OnAerial()
    {
    }
    void OnLanding()
    {
        if(rb.velocity.y < 0)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    [SerializeField] PlayerComponent currentOverride = null;
    [HideInInspector] public bool overrideLockIsActive = false;
    public void NotifyStartOverride(PlayerComponent newOverride, bool lockOverride = false)
    {
        //Any movement-related classes should call this when they're changing movement values.
        //If any of these classes starts changing values, this override will notify all relevant classes.

        if (overrideLockIsActive)
            return;
        overrideLockIsActive = lockOverride;

        if (newOverride == currentOverride)
            return;

        currentOverride = newOverride;                  //Save the most recent override
        onMovementOverrideStart.Invoke(newOverride);    //notify all that a new one has been chosen.
    }
    public void NotifyStopOverride(PlayerComponent stoppingOverride)
    {
        if (stoppingOverride != currentOverride)
            return;

        currentOverride = null;
        overrideLockIsActive = false;
        onMovementOverrideStop.Invoke(stoppingOverride);
    }

    public bool MovementIsOverridenByOther(PlayerComponent origin)
    {
        if (overrideLockIsActive == true || origin == currentOverride)
            return true;

        return false;
    }


    public override void OnPlayerStart(Player pPlayer)
    {
        GetAllComponents();
        groundCheck.onGroundLeave.AddListener(OnAerial);
        groundCheck.onLanding.AddListener(OnLanding);
    }


    public override void OnPlayerFixedUpdate()
    {        rb.useGravity = !groundCheck.isGrounded;    //Player won't use gravity when grounded to prevent sliding down slopes.

        DoMove();
    }

    public override void OnPlayerUpdate()
    {
        Vector2 oldInputs = inputs;
        inputs = GetInput();

        if (oldInputs == Vector2.zero && inputs != Vector2.zero)
        {
            //Wasn't moving before but is now --> start moving.
            onStartMove?.Invoke();
        }
        else if (oldInputs != Vector2.zero && inputs == Vector2.zero)
        {
            //Was moving before but isn't now --> stop moving.
            onStopMove?.Invoke();
        }
    }

    void UpdateSlopeTransform()
    {
        slopeT.position = transform.position;
        slopeT.rotation = transform.rotation;
        slopeT.up = groundCheck.currentSlopeNormal;
    }

    Vector2 GetInput()
    {
        //Raw Axis ensure the value is always 0 or 1. (The standard gradually changes the value... we don't want that).
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        return new Vector2(x, y);
    }

    void GetAllComponents()
    {
        TryGetComponent<Rigidbody>(out rb);
        TryGetComponent<PlayerFPSGroundCheck>(out groundCheck);

        slopeT = new GameObject("test").transform;
    }
}
