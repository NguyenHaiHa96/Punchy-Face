using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private void FollowTarget()
    {
        if (!GameManager.instance.Player.GetComponent<Player>().RagdollActivated)
            transform.position = GameManager.instance.Player.transform.position + offset;
        else
            transform.position = GameManager.instance.Player.GetComponent<Player>().Spine.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }
}
