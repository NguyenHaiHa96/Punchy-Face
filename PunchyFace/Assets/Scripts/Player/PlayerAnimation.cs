using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Player player;
    private Animator animator;

    // Start is called before the first frame update
    void OnEnable()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        player.OnMoving += Player_OnMoving;
        player.OnAttacking += Player_OnAttacking;
    }

    private void OnDisable()
    {
        player.OnMoving -= Player_OnMoving;
        player.OnAttacking -= Player_OnAttacking;
    }

    private void Player_OnAttacking()
    {
        animator.SetTrigger("IsAttacking");
    }

    private void Player_OnMoving()
    {
        animator.SetBool("IsMoving", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
