using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject npcEnemyPrefab;
    [SerializeField] private GameObject playerSpawnPosition;
    [SerializeField] private GameObject npcEnemySpawnPosition;

    private GameObject player;
    private GameObject npcEnemy;

    private bool isGameOver;
    private bool levelCompleted;
    private bool isGameStarted;
    private bool isContinued;
    private bool isRestarted;

    private int currentLevel;

    public GameObject Player { get => player; set => player = value; }
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    public bool LevelCompleted { get => levelCompleted; set => levelCompleted = value; }
    public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }
    public bool IsContinued { get => isContinued; set => isContinued = value; }
    public bool IsRestarted { get => isRestarted; set => isRestarted = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartTask();
        StartSpawn();
    }

    public void Init()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public void StartTask()
    {
        levelCompleted = false;
        isGameOver = false;
        isRestarted = false;
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
    }

    public void StartSpawn() 
    {
        player = Instantiate(playerPrefab);
        player.transform.position = new Vector3(playerSpawnPosition.transform.position.x, 
        0f,
        playerSpawnPosition.transform.position.z);
        npcEnemy = Instantiate(npcEnemyPrefab);
        npcEnemy.transform.position = npcEnemySpawnPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerStatus();
        CheckNPCEnemyStatus();  
        GameState();
    }

    public void CheckPlayerStatus()
    {
        if (player.TryGetComponent(out Player playerInfo))
        {
            if (playerInfo.IsDeath)
            {
                Vector3 newPlayerSpawnPosition = new Vector3(playerInfo.Spine.transform.position.x, 
                0f, 
                playerInfo.Spine.transform.position.z) ;
                player = Instantiate(playerPrefab);
                player.transform.position = newPlayerSpawnPosition;
            }
        }
    }

    public void CheckNPCEnemyStatus()
    {
        if (npcEnemy.TryGetComponent(out NPCEnemy npcEnemyInfo))
        {
            if (npcEnemyInfo.IsDeath)
            {
                Vector3 newNPCSpawnPosition = npcEnemyInfo.Spine.transform.position;
                npcEnemy = Instantiate(npcEnemyPrefab);
                npcEnemy.transform.position = newNPCSpawnPosition;
            }
        }
    }

    public void GameState() 
    {
        if (levelCompleted)
        {
            UIManager.instance.ShowLevelCompletePanel(true);
        }

        if (IsGameOver)
        {
            UIManager.instance.ShowGameOverPanel(true);
            TapToReStart();
        }
    }   

    IEnumerator TapToReStart()
    {
        yield return new WaitForSeconds(0.5f);

        if (Input.GetMouseButton(0))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }           
    }
}
