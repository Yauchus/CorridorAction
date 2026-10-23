using UnityEngine;
using System.Collections;

using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public int enemyCount = 10;
    public float spacing = 2.5f;

    void Start()
    {
        StartCoroutine(SpawnEnemiesWithDelay());
    }

    IEnumerator SpawnEnemiesWithDelay()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy Prefab is not assigned in the EnemySpawner");
            yield break;
        }

        // Calculate the starting position to center the line of enemies
        Vector3 startPosition = transform.position - (transform.right * (enemyCount - 1) * spacing * 0.5f);

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = startPosition + (transform.right * i * spacing);

            Instantiate(enemyPrefab, spawnPos, transform.rotation);

            // Random delay before spawning the next enemy (0.3 to 0.8 seconds)
            float randomWait = Random.Range(0.3f, 0.8f);
            yield return new WaitForSeconds(randomWait);
        }

        Debug.Log("All enemies have been successfully spawned with delay");
    }
}

