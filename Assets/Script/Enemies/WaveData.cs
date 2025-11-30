using UnityEngine;

[System.Serializable]
public class WaveData
{
    [Header("몬스터 종류 (Enemy Prefab)")]
    public GameObject enemyPrefab;

    [Header("웨이브 몬스터 수")]
    [Min(1)] public int enemyCount = 10;

    [Header("스폰 간격(초)")]
    [Min(0.05f)] public float spawnInterval = 1f;

    [Header("웨이브 지속 시간(초)")]
    [Min(1f)] public float duration = 30f;

    [Header("몬스터 속도 배율")]
    [Min(0.1f)] public float speedMultiplier = 1f;

    [Header("몬스터 체력 (HP)")]
    [Min(1f)] public float health = 40f;
}
