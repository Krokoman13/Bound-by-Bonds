using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimations : PlayerComponent
{
    [SerializeField] Animator anim = null;

    [SerializeField] string animBoolName_Moving = "Moving";
    [SerializeField] string animBoolName_Crouching = "Crouching";
    [SerializeField] string animBoolName_Sprinting = "Sprinting";
    [SerializeField] string animTriggerName_Jump = "Jump";

    public void ANIM_Move(bool newState)
    {
        anim.SetBool(animBoolName_Moving, newState);
    }

    public void ANIM_Jump()
    {
        anim.SetTrigger(animTriggerName_Jump);
    }

    public void ANIM_Sprint(bool newState)
    {        
        anim.SetBool(animBoolName_Sprinting, newState);
    }

    public void ANIM_Crouch(bool newState)
    {
        anim.SetBool(animBoolName_Crouching, newState);
    }

}