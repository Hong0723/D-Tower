using UnityEngine;

[DisallowMultipleComponent]
public class Tower : MonoBehaviour
{
    [Header("기본 설정")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float fireInterval = 0.7f;
    [SerializeField] private float range = 4.5f;
    [SerializeField] private int damage = 5;

    [Header("업그레이드 설정")]
    [SerializeField] private int maxLevel = 5;

    private int currentLevel = 1;
    private float timer;

    public int CurrentLevel => currentLevel;

    public void SetLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 1, maxLevel);
        for (int i = 1; i < currentLevel; i++)
        {
            damage += 3;
            range += 0.3f;
            fireInterval = Mathf.Max(0.2f, fireInterval - 0.05f);
        }
        Debug.Log($"타워 생성! 레벨 {currentLevel}, 데미지 {damage}, 사거리 {range}");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < fireInterval) return;

        var target = FindNearestEnemyInRange();
        if (target != null)
        {
            var p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            p.damage = damage;
            p.Init(target);
            timer = 0f;
        }
    }

    private Transform FindNearestEnemyInRange()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies == null || enemies.Length == 0) return null;

        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < bestDist && d <= range)
            {
                best = e.transform;
                bestDist = d;
            }
        }
        return best;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}