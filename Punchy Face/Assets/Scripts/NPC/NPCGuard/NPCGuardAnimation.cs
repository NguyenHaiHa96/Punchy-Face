using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGuardAnimation : MonoBehaviour
{
    private NPCGuard npc;
    private Animator animator;

    // Start is called before the first frame update
    void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
        npc = GetComponent<NPCGuard>();
        npc.OnAttacking += Npc_OnAttacking;
    }

    private void OnDisable()
    {
        npc.OnAttacking -= Npc_OnAttacking;
    }

    private void Npc_OnAttacking()
    {
        animator.SetTrigger("IsAttacking");
    }
}
