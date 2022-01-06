using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private MeshRenderer mr;
    private BoxCollider col;
    private bool disabled;

    public bool IsVisible => disabled == false;



    private void OnEnable()
    {
        disabled = false;
        mr = GetComponent<MeshRenderer>();
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDisable(bool v)
    {
        disabled = v;
        if (disabled)
        {
            mr.enabled = false;
            col.enabled = false;
        }

        StartCoroutine(ReActive());
    }

    IEnumerator ReActive()
    {
        yield return new WaitForSeconds(4f);
        disabled = false;
        mr.enabled = true;
        col.enabled = true;
    }
}
