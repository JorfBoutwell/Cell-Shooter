using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public WeaponManager weaponScript;
    public PlayerControllerNEW movementScript;

    public WeaponManager.WeaponState state;

    public Image displaySprite; //set to different sprite when player switches character
    public Animator displayAnimator; //plays animations; set to different when player switches character

    //Animator cameraAnimator (when we implement camera animations during movement)

    private void Awake() {

    }

    private void Update() {
        WeaponAnimationController(weaponScript.state);
    }

    public void WeaponAnimationController(WeaponManager.WeaponState state)
    {
        switch(state)
        {
            case WeaponManager.WeaponState.idle:
                if (!displayAnimator.GetCurrentAnimatorStateInfo(0).IsName("NeuronIdle")) displayAnimator.Play("NeuronIdle");
                break;
            case WeaponManager.WeaponState.shooting:
                if (!displayAnimator.GetCurrentAnimatorStateInfo(0).IsName("NeuronPrimaryFire")) displayAnimator.Play("NeuronPrimaryFire");
                break;
            case WeaponManager.WeaponState.reloading:
                if (!displayAnimator.GetCurrentAnimatorStateInfo(0).IsName("NeuronReload_Temp")) displayAnimator.Play("NeuronReload_Temp");
                break;  
            default: return;
        }
    }

    private void Update()
    {
       
    }

    public void WeaponAnimator(WeaponManager.WeaponState state)
    {
        switch (state)
        {
            case WeaponManager.WeaponState.shooting:
                break;
            default: break;

        }
    }
}
