using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public event Action OnMoving;
    public event Action OnAttacking;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject animated;
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private GameObject spine;
    [SerializeField] private Transform victoryPosition;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider col;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float radius;
    [SerializeField] private float radiusCheck;
    [SerializeField] private float attackInterval;
    [SerializeField] private int currentPowerLevel;
    [SerializeField] private UIFloatingPlayer uiFloating;

    private TakeForcePoint takeForcePoint;
    private Vector3 targetPosition;
    private Vector3 lookAtPosition;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private Quaternion playerRotation;
    
    private float scaleAddition;
    private float elapsedTime;
    private int nextPowerUp;
    private int currentPowerUp;
    private bool ragdollActivated = false;
    private bool isControlling = true;
    private bool moving;
    private bool isAttacking;
    private bool isDeath;

    public int CurrentPowerLevel { get => currentPowerLevel; set => currentPowerLevel = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    internal bool IsDeath { get => isDeath; set => isDeath = value; }
    public GameObject Spine { get => spine; set => spine = value; }
    public bool RagdollActivated { get => ragdollActivated; set => ragdollActivated = value; }
    public GameObject Ragdoll { get => ragdoll; set => ragdoll = value; }

    private void OnEnable()
    {
        takeForcePoint = spine.GetComponent<TakeForcePoint>();
        uiFloating = GetComponent<UIFloatingPlayer>();
        victoryPosition = Level.LastChild;
        rb.isKinematic = true;
        elapsedTime = 0f;
        scaleAddition = 1 + currentPowerLevel * 0.1f;
        nextPowerUp = 6;
        StartScaleSize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) 
            if (isControlling)
                SetTargetPosition();
           
        if (moving)
        {
            OnMoving();
            Rotate();
            ScaleUp();       
        }
        CheckCollider();
    }

    public void StartScaleSize()
    {
        transform.localScale = Vector3.one * (1 + (float) currentPowerLevel / 10);
    }

    public void ReachCheckPoint() 
    {
        agent.enabled = true;
        agent.SetDestination(victoryPosition.position);
    }

    public void OnDeath(Vector3 direction, Vector3 hitPoint)
    {
        uiFloating.IsDisable = true;
        agent.speed = 0;
        ragdollActivated = true;
        animated.SetActive(false);
        ragdoll.SetActive(true);
        takeForcePoint.ReceiveForce(direction, hitPoint);
        col.enabled = false;

        StartCoroutine(ConfirmDeath());
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
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
    private void FixedUpdate()
    {
        if (moving) 
        {
            CalculateVelocity();
            rb.velocity = this.velocity * Time.deltaTime;
        }
    }

    private void ScaleUp()
    {
        if (currentPowerUp >= nextPowerUp)
        {
            scaleAddition += 0.1f;
            this.transform.localScale = Vector3.one * scaleAddition;          
            currentPowerUp = 0;
            currentPowerLevel += 1;
        }   
    }

    public void ScaleDown(float scale)
    {
        this.transform.localScale = Vector3.one * (1 + scale);
    }

    private void SetTargetPosition()
    {
        rb.isKinematic = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (!hit.transform.gameObject.CompareTag("Player"))
            {
                targetPosition = hit.point;
                lookAtPosition = new Vector3(targetPosition.x - this.transform.position.x,
                    this.transform.position.y,
                    targetPosition.z - this.transform.position.z);
                moving = true;
                playerRotation = Quaternion.LookRotation(lookAtPosition);       
            }             
        }
    }

    private void Rotate()
    {
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, 
            playerRotation,
            rotationSpeed * Time.deltaTime));
    }

    private void CalculateVelocity()
    {
        moveDirection = lookAtPosition.normalized;
        velocity = moveDirection * moveSpeed;
    }

    private void CheckCollider()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius);
        foreach (var collider in hitColliders)
        {
            if (!collider.gameObject.CompareTag("Player"))
            {
                if (collider.gameObject.CompareTag("NPCEnemy") || collider.gameObject.CompareTag("NPCGuard"))
                {
                    if (Time.time > elapsedTime)
                    {
                        OnAttacking();
                        elapsedTime = Time.time + attackInterval;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("RedPowerUp")) 
        {                     
            if (other.TryGetComponent(out PowerUp powerUp))
            {
                if (powerUp.IsVisible)
                {
                    powerUp.SetDisable(true);
                    currentPowerUp++;
                }
            }         
        }

        if (other.gameObject.CompareTag("CheckPoint")) 
        {
            isControlling = false;
            ReachCheckPoint();    
        }

        if (other.gameObject.CompareTag("VictoryPosition"))
        {
            GameManager.instance.LevelCompleted = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
