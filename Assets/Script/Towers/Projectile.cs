using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [Header("Flight")]
    public float speed = 8f;
    public float hitRadius = 0.01f;      // 거리 판정 여유
    public float maxLifetime = 6f;      // 고아 탄 방지

    [Header("Damage")]
    public int damage = 5;

    [Header("Effects")]
    public GameObject hitVFXPrefab;

    private Transform target;
    private Rigidbody2D rb;
    private float life;
    private float lastDist = Mathf.Infinity;


    public void Init(Transform t)
    {
        target = t;
        if (target == null) Destroy(gameObject);
    }

    // 풀링 대비 초기화
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.gravityScale = 0f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true; // 충돌은 Trigger로만
    }

    private void OnEnable()
    {
        life = 0f;
    }


    private void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        life += Time.fixedDeltaTime;
        if (life > maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        // ---- 방향 회전 처리 ----
        Vector2 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // ---- 이동 ----
        Vector2 next = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);

        if (rb) rb.MovePosition(next);
        else transform.position = next;

        float currentDist = Vector2.Distance(next, target.position);
        // 핵심: "지나쳤을 때" 명중 처리
        if (currentDist > lastDist)
        {
            ApplyDamage(target);
            Destroy(gameObject);
            return;
        }

        lastDist = currentDist;
    }

    // 콜라이더가 맞닿는 경우(Trigger)에도 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (target == null) return;

        // 타깃 콜라이더거나, Health 달린 적과 닿았으면 히트 처리
        if (other.transform == target || other.GetComponent<Health>() != null)
        {
            ApplyDamage(other.transform);
            Destroy(gameObject);
        }
    }

    private void ApplyDamage(Transform victim)
    {
        if (hitVFXPrefab != null)
        {
            Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
        }
        if (victim == null) return;
        var h = victim.GetComponent<Health>();
        if (h != null) h.TakeDamage(damage);
    }
}
