using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] pathPoints;

    private bool isSpawning = false;

    private GameObject currentEnemyPrefab;
    private float currentSpeedMultiplier;
    private float currentHealth;

    public void StartWave(GameObject enemyPrefab, int enemyCount, float spawnInterval, float speedMultiplier, float health)
    {
        currentEnemyPrefab = enemyPrefab;
        currentSpeedMultiplier = speedMultiplier;
        currentHealth = health;

        isSpawning = true;
        StartCoroutine(SpawnEnemies(enemyCount, spawnInterval));
    }

    public void StopWave()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnEnemies(int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            if (!isSpawning) yield break;

            GameObject enemy = Instantiate(currentEnemyPrefab, spawnPoint.position, Quaternion.identity);

            var mover = enemy.GetComponent<EnemyMover>();
            if (mover != null)
                mover.SetSpeedMultiplier(currentSpeedMultiplier);

            var health = enemy.GetComponent<Health>();
            if (health != null)
                health.SetHealth(currentHealth);

            mover?.Init(pathPoints);

            yield return new WaitForSeconds(interval);
        }
    }
}
