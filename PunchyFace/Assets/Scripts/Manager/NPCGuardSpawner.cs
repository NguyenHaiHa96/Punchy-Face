using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGuardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcGuardSpawnPosition;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private NPCGuard npcGuardPrefab;
    [SerializeField] private NPCGuard[] npcGuards;

    private int numberOfGuards;

    // Start is called before the first frame update
    void Start()
    {
        StartTask();
    }

    private void StartTask()
    {
        spawnPositions = new Transform[npcGuardSpawnPosition.transform.childCount];
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            spawnPositions[i] = npcGuardSpawnPosition.transform.GetChild(i);
        }
        numberOfGuards = GameManager.instance.CurrentLevel;
        npcGuards = new NPCGuard[numberOfGuards];
        for (int i = 0; i < numberOfGuards; i++)
        {
            var newGuard = Instantiate(npcGuardPrefab);
            newGuard.transform.position = spawnPositions[i].transform.position;
            newGuard.CurrentPowerLevel = i + 1;
            newGuard.StartScaleSize();
            npcGuards[i] = newGuard;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckNPCGuardStatus();
    }

    private void CheckNPCGuardStatus()
    {
        for (int i = 0; i < npcGuards.Length; i++)
        {
            if (npcGuards[i].IsDeath)
            {
                //Vector3 startPosition = npcGuards[i].StartPosition;
                int currentPowerLevel = npcGuards[i].CurrentPowerLevel;
                npcGuards[i] = Instantiate(npcGuardPrefab);
                npcGuards[i].transform.position = spawnPositions[i].transform.position;
                npcGuards[i].CurrentPowerLevel = currentPowerLevel;
                npcGuards[i].StartScaleSize();
            }
        }
    }
}
