using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorgiEnemy : EnemyFollow
{
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>(); //grab Animator once
    }

    protected override void Chase()
    {
        base.Chase();//calling chase 

        // Just handle animation (no logic changes)
        if (animator != null)
        {
            bool isMoving = _player != null && Vector3.Distance(transform.position, _player.transform.position) > 1.5f;
            animator.SetBool("IsWalking", isMoving);
        }
    }
}


