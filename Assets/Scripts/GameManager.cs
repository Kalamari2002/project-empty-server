using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    List<Transform> spawnPoints = new List<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject spawnPointHolder = GameObject.FindGameObjectWithTag("SpawnPoints");
        foreach (Transform spawnPoint in spawnPointHolder.GetComponentInChildren<Transform>())
        {
            if (spawnPoint != spawnPointHolder)
            {
                spawnPoints.Add(spawnPoint);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)]; 
    }
}
