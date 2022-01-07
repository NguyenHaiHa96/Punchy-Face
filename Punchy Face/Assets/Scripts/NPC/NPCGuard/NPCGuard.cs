using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGuard : MonoBehaviour
{
    public event Action OnAttacking;

    [SerializeField] private GameObject animated;
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private GameObject spine;
    [SerializeField] private BoxCollider col;
    [SerializeField] private int currentPowerLevel;
    [SerializeField] private float radiusCheckCollider;
    [SerializeField] private UIFloatingNPCEnemy uiFloating;

    private Vector3 startPosition;
    private TakeForcePoint takeForcePoint;
    private bool isDeath;

    public int CurrentPowerLevel { get => currentPowerLevel; set => currentPowerLevel = value; }
    public GameObject Spine { get => spine; set => spine = value; }
    public bool IsDeath { get => isDeath; set => isDeath = value; }
    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }

    void OnEnable()
    {
        startPosition = transform.position;
        takeForcePoint = spine.GetComponent<TakeForcePoint>();
        uiFloating = GetComponent<UIFloatingNPCEnemy>();
        StartScaleSize();
    }

    public void OnDeath(Vector3 direction, Vector3 hitPoint)
    {
        animated.SetActive(false);
        ragdoll.SetActive(true);
        takeForcePoint.ReceiveForce(Vector3.left, hitPoint);
        col.enabled = false;

        StartCoroutine(ConfirmDeath());
    }

    public void StartScaleSize()
    {      
        transform.localScale = Vector3.one * (1 + (float) currentPowerLevel / 10);
    }

    IEnumerator ConfirmDeath()
    {
        yield return new WaitForSeconds(3f);
        ragdoll.SetActive(false);
        isDeath = true;

        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    private void CheckCollider()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radiusCheckCollider);
        foreach (var collider in hitColliders)
        {
            if (!collider.gameObject.CompareTag("NPCGuard"))
            {
               if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("NPCEnemy")) 
               {
                   OnAttacking?.Invoke();
               }
            }
         }
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollider();
    }
}
