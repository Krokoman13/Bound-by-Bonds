using UnityEngine;

public class PlayerFPSAnimations : PlayerComponent
{
    Animator anim = null;
    string animName_move = "Move";
    string animName_landing = "Landing";
    string animName_jump = "Jump";



    public void ANIM_Landing()
    {
        anim.ResetTrigger(animName_jump);
        anim.SetTrigger(animName_landing);
    }
    public void ANIM_Jump()
    {
        anim.ResetTrigger(animName_landing);
        anim.SetTrigger(animName_jump);
    }
    public void ANIM_Move(bool isMoving)
    {
        anim.SetBool(animName_move, isMoving);
    }


    public override void OnPlayerStart(Player pPlayer)
    {
        GetAllComponents();
    }

    void GetAllComponents()
    {
        TryGetComponent<Animator>(out anim);
    }
}