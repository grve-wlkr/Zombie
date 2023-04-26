using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private float spawnInterval = 5f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            int randomSpawnIndex = Random.Range(0, spawnLocations.Length);
            Transform spawnLocation = spawnLocations[randomSpawnIndex];

            Instantiate(enemyPrefab, spawnLocation.position, spawnLocation.rotation);
        }
    }
}
