using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs;
    [SerializeField] private GameObject[] disabledPowerUp;
    [SerializeField] private GameObject powerUpHolder;
    [SerializeField] private float spacing;
    [SerializeField] private float numberOfRows;
    [SerializeField] private float numberOfColumns;

    private float newXposition;
    private float newZPosition;
    private int selectedPrefab;
   

    // Start is called before the first frame update
    void Start()
    {
        GeneratePowerUps();
    }

    private void GeneratePowerUps()
    {
        newXposition = transform.position.x;
        newZPosition = transform.position.z;

        for (int i = 0; i < numberOfRows; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
            {
                selectedPrefab = Random.Range(0, 2);
                GameObject newPowerUp = Instantiate(powerUpPrefabs[selectedPrefab], powerUpHolder.transform);
                newPowerUp.transform.position = new Vector3(newXposition, newPowerUp.transform.position.y, newZPosition);
                newXposition += spacing;
            }
            newXposition = transform.position.x;
            newZPosition -= spacing;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
