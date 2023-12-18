using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float health = 10;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(health <= 0)
        {
            animator.Play("DummyDeath");
            health = 10;
        }
    }
}
