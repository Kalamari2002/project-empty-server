using System.Collections.Generic;
using UnityEngine;


public class FishSpawner : MonoBehaviour
{
    [SerializeField] int amountOfFish;
    [SerializeField] List<GameObject> fish = new List<GameObject>();
    [SerializeField] float minScale = 0.5f;
    [SerializeField] float maxScale = 3;

    BoxCollider boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        SpawnFish();
    }

    void SpawnFish()
    {
        for (int i = 0; i < amountOfFish; i++) 
        {
            Vector3 spawnPosition = transform.position + (transform.right * Random.Range(-boxCollider.size.x / 2, boxCollider.size.x / 2)) 
                + (transform.up * Random.Range(-boxCollider.size.y / 2, boxCollider.size.y / 2)) 
                + (transform.forward * Random.Range(-boxCollider.size.z / 2, boxCollider.size.z / 2));
            GameObject fishInstance = Instantiate(fish[Random.Range(0, fish.Count)], spawnPosition, Quaternion.identity);
            float scale = Random.Range(minScale, maxScale);
            fishInstance.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
