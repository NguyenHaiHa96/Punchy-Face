using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitMarker : MonoBehaviour
{
    private SphereCollider col;
    private Player player;

    private void OnEnable()
    {
        col = GetComponent<SphereCollider>();
        player = GetComponentInParent<Player>();
        
    }
    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.CompareTag("NPCEnemy"))
            {
                if (other.TryGetComponent(out NPCEnemy npcEnemy))
                {
                    if (player.CurrentPowerLevel >= npcEnemy.CurrentPowerLevel)
                    {
                        Vector3 subDirection = new Vector3(npcEnemy.transform.position.x - transform.position.x, 
                            0f, 
                            npcEnemy.transform.position.z - transform.position.z);
                        Vector3 direction = subDirection.normalized;
                        npcEnemy.OnDeath(direction, npcEnemy.Spine.transform.position);                       
                            
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.forward, out hit))
                        {
                           
                        }
                    }                 
                }
            }
            if (other.gameObject.CompareTag("NPCGuard"))
            {
                if (other.TryGetComponent(out NPCGuard npcGuard))
                {
                    if (player.CurrentPowerLevel >= npcGuard.CurrentPowerLevel)
                    {
                        Vector3 subDirection = Vector3.left;
                        Vector3 direction = subDirection.normalized;
                        npcGuard.OnDeath(direction, npcGuard.Spine.transform.position);

                        float scale = player.CurrentPowerLevel - npcGuard.CurrentPowerLevel == 0
                            ? 0
                            : (player.CurrentPowerLevel - npcGuard.CurrentPowerLevel) / 10;
                        player.ScaleDown(scale);

                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.forward, out hit))
                        {
                            
                        }
                    }
                }
            }

        }
    }
}
