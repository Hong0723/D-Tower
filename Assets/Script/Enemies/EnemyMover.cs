using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isBoss = false;

    private Transform[] waypoints;
    private int currentIndex = 0;
    private float speedMultiplier = 1f;

    public void Init(Transform[] points)
    {
        waypoints = points;
        currentIndex = 0;
        if (waypoints != null && waypoints.Length > 0)
            transform.position = waypoints[0].position;
    }

    public void Setup(Transform[] points) => Init(points);

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

        Transform target = waypoints[currentIndex];
        float currentSpeed = speed * speedMultiplier;
        transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                ReachEnd();
            }
        }
    }

    private void ReachEnd()
    {
        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP != null)
        {
            if (isBoss)
            {
                playerHP.TakeDamage(playerHP.CurrentHP);
            }
            else
            {
                playerHP.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}