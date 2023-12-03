using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public WeaponManager weaponScript;
    public PlayerControllerNEW movementScript;

    public Image displaySprite; //set to different sprite when player switches character
    public Animator displayAnimator; //plays animations; set to different when player switches character

    //Animator cameraAnimator (when we implement camera animations during movement)

    private void Awake() {

       displaySprite = GetComponent<Image>();
       displayAnimator = GetComponent<Animator>();
    }
}
