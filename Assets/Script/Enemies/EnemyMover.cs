using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isBoss = false;

    private Transform[] waypoints;
    private int currentIndex = 0;
    private float speedMultiplier = 1f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Init(Transform[] points)
    {
        waypoints = points;
        currentIndex = 0;
        if (waypoints != null && waypoints.Length > 0)
            transform.position = waypoints[0].position;

        UpdateFacing();
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void Knockback(Vector3 force)
    {
        transform.position += force;
    }

    private void Update()
    {
        if (waypoints == null || currentIndex >= waypoints.Length) return;

        float currentSpeed = speed * speedMultiplier;
        Transform target = waypoints[currentIndex];

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentIndex++;
            UpdateFacing();

            if (currentIndex >= waypoints.Length)
                ReachEnd();
        }
    }

    private void UpdateFacing()
    {
        if (currentIndex < 3) sr.flipX = false; // 0~2
        else if (currentIndex < 7) sr.flipX = true; // 2~6
        else if (currentIndex < 13) sr.flipX = false; // 6~12
        else if (currentIndex < 15) sr.flipX = true; // 12~14
        else if (currentIndex < 19) sr.flipX = false; // 14~18
        else if (currentIndex < 25) sr.flipX = true; // 18~23
        else sr.flipX = false; // 23~27
    }

    private void ReachEnd()
    {
        PlayerHP player = FindObjectOfType<PlayerHP>();

        if (player != null)
        {
            if (isBoss)
                player.TakeDamage(10); // 보스 통과시 10 데미지
            else
                player.TakeDamage(damage);  // 일반 몬스터는 damage 사용
        }

        Destroy(gameObject);
    }

}
