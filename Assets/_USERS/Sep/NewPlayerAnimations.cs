using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(PlayerFPSMovement))]
[RequireComponent(typeof(PlayerFPSJump))]
[RequireComponent(typeof(PlayerFPSGroundCheck))]
[RequireComponent(typeof(PlayerFPSSprinting))]
public class NewPlayerAnimations : PlayerComponent
{
    [SerializeField] Transform camTarget = null;
    Vector3 offset = new Vector3(0, 0.4f, 0);

    public enum AnimState { Idle, Walking, Sprinting, Crouching, Crawling, Falling };
    public AnimState animState = AnimState.Falling;


    PlayerFPSMovement movement = null;
    PlayerFPSJump jump = null;
    PlayerFPSGroundCheck groundcheck = null;
    PlayerFPSCrouching crouch = null;
    PlayerFPSSprinting sprint = null;



    [SerializeField] float breatheFrequency_idle = .85f;
    [SerializeField] float breatheAmplitude_idle = .065f;

    [SerializeField] float breatheFrequency_walk = 12f;
    [SerializeField] float breatheAmplitude_walk = .065f;

    [SerializeField] float breatheFrequency_crawl = 8f;
    [SerializeField] float breatheAmplitude_crawl = .065f;
    [Space(8)]
    [SerializeField] float breatheFrequency_sprint = 16f;
    [SerializeField] float breatheAmplitude_sprint = .065f;

    public UnityEvent onStep_Walk = null;



    public override void OnPlayerUpdate()
    {
        float freq = 0;
        float ampl = 0;


        switch (animState)
        {
            case AnimState.Idle:
                freq = breatheFrequency_idle;
                ampl = breatheAmplitude_idle;
                break;
            case AnimState.Walking:
                freq = breatheFrequency_walk;
                ampl = breatheAmplitude_walk;
                break;
            case AnimState.Sprinting:
                freq = breatheFrequency_sprint;
                ampl = breatheAmplitude_sprint;
                break;
            case AnimState.Crouching:
                break;
            case AnimState.Crawling:
                freq = breatheFrequency_crawl;
                ampl = breatheAmplitude_crawl;
                break;
            case AnimState.Falling:
                break;
            default:
                break;
        }


        Vector3 test = transform.position - Vector3.up * ampl;
        //print(camTarget.position.y);
        if (camTarget.position == test)
        {

        }



        float newHeight = transform.position.y + Mathf.Cos(Time.time * freq) * ampl;
        //print(Mathf.Cos(Time.time * freq));
        if(Mathf.Cos(Time.time * freq) < -.95)
        {
            if (!stepDone)
                onStep_Walk?.Invoke();

            stepDone = true;
        }
        else
        {
            stepDone = false;
        }
        
        camTarget.position = new Vector3(camTarget.position.x, newHeight, camTarget.position.z) + offset;

        GetAnimState();
    }
    bool stepDone = false;


    void GetAnimState()
    {
        if (crouch.isCurrentlyCrouching)
        {
            animState = AnimState.Crouching;
            if (movement.isMoving)
                animState = AnimState.Crawling;
            return;
        }
        else if (sprint.isSprinting)
        {
            animState = AnimState.Sprinting;
            return;
        }


        if (groundcheck.isGrounded)
        {
            if (movement.isMoving)
                animState = AnimState.Walking;
            else
                animState = AnimState.Idle;
            return;
        }


        animState = AnimState.Falling;
    }



    public override void OnPlayerStart(Player pPlayer)
    {
        TryGetComponent<PlayerFPSMovement>(out movement);
        TryGetComponent<PlayerFPSJump>(out jump);
        TryGetComponent<PlayerFPSGroundCheck>(out groundcheck);
        TryGetComponent<PlayerFPSCrouching>(out crouch);
        TryGetComponent<PlayerFPSSprinting>(out sprint);
    }

}