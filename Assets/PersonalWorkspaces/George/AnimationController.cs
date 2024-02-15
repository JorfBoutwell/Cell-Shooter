using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public Image displaySprite; //set to different sprite when player switches character
    public Animator displayAnimator; //plays animations; set to different when player switches character

    public Animator cameraAnimator;

    public void WeaponAnimationController(WeaponManager.WeaponState state)
    {
        switch (state)
        {
            case WeaponManager.WeaponState.idle:
                Debug.Log("animating idle");
                displayAnimator.Play("NeuronIdle");
                break;
            case WeaponManager.WeaponState.shooting:
                displayAnimator.Play("NeuronPrimaryFire");
                break;
            case WeaponManager.WeaponState.reloading:
                
                if (!displayAnimator.GetCurrentAnimatorStateInfo(0).IsName("NeuronReload_Temp"))
                {
                    displayAnimator.Play("NeuronReload_Temp");
                    Debug.Log("reloadanim");
                }
                break;
            case WeaponManager.WeaponState.ability1:
                Debug.Log("playing ability");
                displayAnimator.Play("Neuron_Ability1");
                break;
            default: return;
        }
    }

    public void MovementAnimationController(PlayerControllerNEW.MovementState state, bool wallLeft, bool wallRight)
    {
        switch (state)
        {
            case PlayerControllerNEW.MovementState.idle:
                cameraAnimator.Play("Idle");
                break;
            case PlayerControllerNEW.MovementState.wallrunning:
                if (wallLeft)
                {
                    cameraAnimator.Play("EnterWallRun_Right");
                }
                else
                {
                    cameraAnimator.Play("EnterWallRun_Left");
                }
                break;
            case PlayerControllerNEW.MovementState.exitingwall:
                if (wallLeft)
                {
                    cameraAnimator.Play("ExitWallRun_Left");
                }
                else
                {
                    cameraAnimator.Play("ExitWallRun_Right");
                }
                break;
            case PlayerControllerNEW.MovementState.sprinting:
                cameraAnimator.Play("Sprint");
                break;
            case PlayerControllerNEW.MovementState.walking:
                cameraAnimator.Play("Walk");
                break;
            case PlayerControllerNEW.MovementState.air:
                cameraAnimator.Play("Air");
                break;
            default: return;
        }
    }
}
