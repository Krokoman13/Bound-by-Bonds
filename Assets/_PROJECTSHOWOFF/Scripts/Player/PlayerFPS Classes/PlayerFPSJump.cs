using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(PlayerFPSGroundCheck))]
public class PlayerFPSJump : PlayerComponent
{
    Rigidbody rb = null;
    PlayerFPSGroundCheck groundCheck = null;

    [Header("Jump Variables")]
    [SerializeField]float jumpForce = 10;


    public UnityEvent onJump = null;

    public void DoJump()
    {
        //Cannot jump mid-air.
        if (!groundCheck.isGrounded)
            return;

        rb.useGravity = true;
        Vector3 jumpDir = transform.up;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(jumpDir * jumpForce, ForceMode.VelocityChange);
        onJump?.Invoke();
    }


    void JumpInput()
    {
        if(Input.GetButtonDown("Jump"))
        {
            DoJump();
        }
    }


    public override void OnPlayerUpdate()
    {        
        JumpInput();
    }
    public override void OnPlayerStart(Player pPlayer)
    {        
        GetAllComponents();
    }

    void GetAllComponents()
    {
        TryGetComponent<Rigidbody>(out rb);
        TryGetComponent<PlayerFPSGroundCheck>(out groundCheck);
    }
}