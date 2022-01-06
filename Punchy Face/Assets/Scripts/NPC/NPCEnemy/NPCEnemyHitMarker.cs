using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEnemyHitMarker : MonoBehaviour
{
    private NPCEnemy npc;

    private void OnEnable()
    {
        npc = GetComponentInParent<NPCEnemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("NPCEnemy"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (other.TryGetComponent(out Player player))
                {
                    if (npc.CurrentPowerLevel > player.CurrentPowerLevel)
                    {
                        Vector3 subDirection = new Vector3(player.transform.position.x - transform.position.x,
                                0f,
                                player.transform.position.z - transform.position.z);
                        Vector3 direction = subDirection.normalized;
                        player.OnDeath(direction, player.Spine.transform.position);
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.forward * 10, out hit))
                        {
                            
                        }
                    }
                }
            }

            if (other.gameObject.CompareTag("NPCGuard"))
            {
                if (other.TryGetComponent(out NPCGuard npcGuard))
                {
                    if (npc.CurrentPowerLevel >= npcGuard.CurrentPowerLevel)
                    {
                        Vector3 subDirection = Vector3.right;
                        Vector3 direction = subDirection.normalized;
                        npcGuard.OnDeath(direction, npcGuard.Spine.transform.position);

                        float scale = npc.CurrentPowerLevel - npcGuard.CurrentPowerLevel == 0
                            ? 0
                            : (npc.CurrentPowerLevel - npcGuard.CurrentPowerLevel) / 10;
                        npc.ScaleDown(scale);

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
