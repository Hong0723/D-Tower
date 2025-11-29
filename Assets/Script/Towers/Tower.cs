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
    [SerializeField] private Sprite[] upgradeSprites; //🎯 단계별 Sprite 저장

    private SpriteRenderer spriteRenderer;
    private int currentLevel = 1;
    private float timer;

    public int CurrentLevel => currentLevel;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyLevelVisual(); // 생성 시 레벨 반영
    }

    /// <summary>
    /// 타워 초기 배치 시 레벨 설정
    /// </summary>
    public void SetLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 1, maxLevel);

        fireInterval = 0.7f;
        range = 4.5f;
        damage = 5;

        // 이전 레벨 누적 능력 적용
        for (int i = 1; i < currentLevel; i++)
        {
            UpgradeStat();
        }

        ApplyLevelVisual();
        Debug.Log($"타워 생성! 레벨 {currentLevel}, 데미지 {damage}, 사거리 {range}");
    }

    /// <summary>
    /// 레벨업 기능 (나중에 버튼과 연결돼도 OK!)
    /// </summary>
    public void LevelUp()
    {
        if (currentLevel >= maxLevel) return;

        currentLevel++;
        UpgradeStat();
        ApplyLevelVisual();

        Debug.Log($"레벨업! 현재 레벨 {currentLevel}, 데미지 {damage}, 사거리 {range}");
    }

    private void UpgradeStat()
    {
        damage += 3;
        range += 0.3f;
        fireInterval = Mathf.Max(0.2f, fireInterval - 0.05f);
    }

    /// <summary>
    /// Sprite 변경 🏹
    /// </summary>
    private void ApplyLevelVisual()
    {
        if (upgradeSprites != null &&
            upgradeSprites.Length >= currentLevel &&
            upgradeSprites[currentLevel - 1] != null)
        {
            spriteRenderer.sprite = upgradeSprites[currentLevel - 1];
        }
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
