using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeForcePoint : MonoBehaviour
{
    [SerializeField] private float force;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ReceiveForce(Vector3 direction, Vector3 hitPoint)
    {
        rb.AddForceAtPosition(direction * force, hitPoint, ForceMode.Impulse);      
        
    }
}
