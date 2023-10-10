using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KillFeedAnimation : MonoBehaviour
{
    [SerializeField]
    private Vector2 transformPosition = Vector2.zero;

    [Range(1.0f, 10.0f), SerializeField]
    private float animationDuration = 1f;

    [SerializeField]
    private Ease animationType = Ease.Linear;

    //[SerializeField]
    //private TweenType _doTweenType = TweenType.MovementOneWay;



    void Start()
    {
        //transformPosition = transform.position;
        Debug.Log(transformPosition);
        transform.DOMoveY(700f, animationDuration);
    }

}
