using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatTween : MonoBehaviour
{
    public Vector3 pos1;
    public Vector3 pos2;
    public float speed;

    private void Start()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOLocalMove(pos2, speed).SetEase(Ease.InOutSine));
        //sequence.Append(transform.DOLocalMove(pos1, speed).SetEase(Ease.InOutSine));


        sequence.SetLoops(-1, LoopType.Yoyo);
        sequence.Play();
    }
}
