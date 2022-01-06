using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCEnemy : MonoBehaviour
{
    public event Action OnAttacking;

    [SerializeField] private GameObject animated;
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private GameObject spine;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private BoxCollider col;
    [SerializeField] private float attackInterval;
    [SerializeField] private float radiusCheckCollider;
    [SerializeField] private float radiusCheckPowerUp;
    [SerializeField] private float currentPowerLevel;
    [SerializeField] private float timeToChangePosition;

    private int nextPowerUp = 6;
    private float scaleAddition = 1.1f;
    private float elapsedTime = 0f;
    private bool isDeath = false;
    private float timer;
    private TakeForcePoint takeForcePoint;
    private Quaternion rotation;
    private Vector3 currentPosition;
    private Vector3 randomPosition;
    private float radius = 10f;
    private int currentPowerUp = 0;
    private bool isAttacking;

    public float CurrentPowerLevel { get => currentPowerLevel; set => currentPowerLevel = value; }
    public GameObject Spine { get => spine; set => spine = value; }
    public bool IsDeath { get => isDeath; set => isDeath = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    void OnEnable()
    {     
        currentPosition = transform.position;
        takeForcePoint = spine.GetComponent<TakeForcePoint>();
    }

    // Update is called once per frame
    void Update()
    {
        CollectPowerUp();
        CheckCollider();
        ScaleUp();   
        TryToWin();   
    }

    private void TryToWin()
    {
        if (currentPowerLevel > 2) 
        {
            agent.SetDestination(Level.LastChild.position);
        }
    }

    public void OnDeath(Vector3 direction, Vector3 hitPoint)
    {
        animated.SetActive(false);
        ragdoll.SetActive(true);
        takeForcePoint.ReceiveForce(direction, hitPoint);
        agent.speed = 0f;
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
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    private void CollectPowerUp()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radiusCheckPowerUp);
        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("BluePowerUp"))
            {
                agent.SetDestination(collider.transform.position);
            }
        }

        StartCoroutine(MoveToRandomPosition());
    }

    private void ScaleUp()
    {
        if (currentPowerUp >= nextPowerUp)
        {
            this.transform.localScale = Vector3.one * scaleAddition;
            scaleAddition += 0.1f;
            currentPowerUp = 0;
            currentPowerLevel += 1;
        }
    }

    public void ScaleDown(float scale)
    {
        this.transform.localScale = Vector3.one * (1 + scale);
    }

    IEnumerator MoveToRandomPosition()
    {
        yield return new WaitForSeconds(2f);
        timer += Time.deltaTime;
            if (RandomPosition(currentPosition, radius, out randomPosition))
            {
                Debug.DrawRay(randomPosition, Vector3.up, Color.black, 1);
                Rotate(randomPosition);
                agent.SetDestination(randomPosition);
            }
       if (timer >= timeToChangePosition)
        {
            CollectPowerUp();
            timer = 0;
        }           
    }

    bool RandomPosition(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    private void Rotate( Vector3 position)
    {
        Vector3 lookAtPosition = new Vector3(position.x - transform.position.x,
                    transform.position.y,
                    position.z - transform.position.z);
        rotation = Quaternion.LookRotation(lookAtPosition);
    }

    private void CheckCollider()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radiusCheckCollider);
        foreach (var collider in hitColliders)
        {
            if (!collider.gameObject.CompareTag("NPCEnemy"))
            {
                if (collider.TryGetComponent(out Player player))
                {
                    if (Time.time > elapsedTime)
                    {
                        isAttacking = true;
                        OnAttacking?.Invoke();
                        elapsedTime = Time.time + attackInterval;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BluePowerUp"))
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

        if (other.gameObject.CompareTag("VictoryPosition"))
        {
            GameManager.instance.IsGameOver = true;
        }
    }
}
