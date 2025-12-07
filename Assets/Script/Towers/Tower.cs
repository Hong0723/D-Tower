using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class Tower : MonoBehaviour
{
    [Header("기본 설정")]
    [SerializeField] private float fireInterval = 0.9f;
    [SerializeField] private float range = 4.5f;
    [SerializeField] private int damage = 5;

    [Header("업그레이드 설정")]
    [SerializeField] private int maxLevel = 5;
    [SerializeField] private Sprite[] levelSprites;   // ★ 1~5단계 스프라이트

    [Header("레벨별 투사체 프리팹")]
    [SerializeField] private Projectile[] levelProjectiles;

    [Header("레벨별 피격 이펙트 프리팹")]
    [SerializeField] private GameObject[] levelHitVFX;

    private int currentLevel = 1;
    private float timer;
    private SpriteRenderer sr;

    public int CurrentLevel => currentLevel;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 1, maxLevel);

        // 기본 능력치 초기화
        fireInterval = 0.9f;
        range = 4.5f;
        damage = 5;

        // ★ 레벨에 따른 능력치 증가
        for (int i = 1; i < currentLevel; i++)
        {
            damage += 3;
            range += 0.3f;
            fireInterval = Mathf.Max(0.2f, fireInterval - 0.05f);
        }

        // ★ 레벨 5는 레이저 모드 — 극도로 빠름 & 반동 없음
        if (currentLevel == 5)
        {
            fireInterval = 0.01f;
            damage = 2;

        }

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (levelSprites != null && levelSprites.Length >= currentLevel)
            sr.sprite = levelSprites[currentLevel - 1];
        else
            Debug.LogWarning($"Tower: 레벨 {currentLevel} 스프라이트가 없음!");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < fireInterval) return;

        var target = FindNearestEnemyInRange();
        if (target != null)
        {
            // ★ 레벨별 다른 투사체 발사
            Projectile proj = Instantiate(
                levelProjectiles[currentLevel - 1],
                transform.position,
                Quaternion.identity);

            proj.damage = damage;
            proj.Init(target);

            // ★ 레벨별 다른 피격 이펙트 적용
            if (levelHitVFX != null && levelHitVFX.Length >= currentLevel)
                proj.hitVFXPrefab = levelHitVFX[currentLevel - 1];


            // Update() 내부, Projectile 생성 바로 아래 줄에 추가
            if (currentLevel == 5)
                AudioManager.Instance?.PlaySFX(AudioManager.Instance.towerShootLv5);
            else
                AudioManager.Instance?.PlaySFX(AudioManager.Instance.towerShootBasic);

            // ★ 1~4레벨만 반동 적용
            if (currentLevel < 5)
                StartCoroutine(RecoilRoutine());

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

    // ============================================================
    // ★★★ 1~4레벨 반동 애니메이션 — 절대 크기 망가지지 않음 ★★★
    // ============================================================
    private IEnumerator RecoilRoutine()
    {
        Vector3 originalScale = transform.localScale; // 원래 크기 저장

        // 1단계: 눌림 효과
        Vector3 squashed = new Vector3(
            originalScale.x * 1.05f,
            originalScale.y * 0.92f,
            originalScale.z
        );
        transform.localScale = squashed;
        yield return new WaitForSeconds(0.04f);

        // 2단계: 반동으로 늘어남
        Vector3 stretched = new Vector3(
            originalScale.x * 0.95f,
            originalScale.y * 1.05f,
            originalScale.z
        );
        transform.localScale = stretched;
        yield return new WaitForSeconds(0.04f);

        // 3단계: 원래 크기로 복구
        transform.localScale = originalScale;
    }

}
