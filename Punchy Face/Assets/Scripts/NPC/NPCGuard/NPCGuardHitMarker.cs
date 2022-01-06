using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGuardHitMarker : MonoBehaviour
{
    private NPCGuard npc;

    private void OnEnable()
    {
        npc = GetComponentInParent<NPCGuard>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("NPCGuard"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (other.TryGetComponent(out Player player))
                {
                    if (npc.CurrentPowerLevel > player.CurrentPowerLevel)
                    {
                        Vector3 subDirection = Vector3.back;
                        Vector3 direction = subDirection.normalized;
                        player.OnDeath(direction, player.Spine.transform.position);
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.forward * 10, out hit))
                        {
                            
                        }
                    }
                }
            }

            if (other.gameObject.CompareTag("NPCEnemy"))
            {
                if (other.TryGetComponent(out NPCEnemy npcEnemy))
                {
                    if (npc.CurrentPowerLevel > npcEnemy.CurrentPowerLevel)
                    {
                            Vector3 subDirection = Vector3.back;
                            Vector3 direction = subDirection.normalized;
                            npcEnemy.OnDeath(direction, npcEnemy.Spine.transform.position);
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.forward * 10, out hit))
                        {
                            
                        }
                    }
                }
            }
        }
    }
}
