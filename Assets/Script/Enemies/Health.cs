using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHP = 40f;
    [SerializeField] private Healthbar healthbarPrefab;
    [SerializeField] private int goldReward = 5;
    [SerializeField] private int scoreReward = 10;

    private float currentHP;
    private Healthbar hb;

    public Healthbar HealthbarPrefab => healthbarPrefab;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void AttachHealthbar(Healthbar bar)
    {
        if (bar == null) return;
        hb = bar;
        hb.Setup(transform, maxHP, currentHP);
    }

    public void SetHealth(float hp)
    {
        maxHP = hp;
        currentHP = hp;

        if (hb != null)
            hb.UpdateHealth(currentHP, maxHP);
    }

    public void TakeDamage(float amount)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0f, maxHP);
        hb?.UpdateHealth(currentHP, maxHP);

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        GoldBank bank = FindObjectOfType<GoldBank>();
        if (bank != null)
        {
            bank.Earn(goldReward);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreReward);
        }

        Destroy(gameObject);
    }

    // Health.cs 안에서 HP바 연결 부분
    public void InitHealthbar()
    {
        if (healthbarPrefab != null)
        {
            var hb = Instantiate(healthbarPrefab, transform); // 부모: Enemy
            hb.Setup(transform, maxHP, currentHP);
        }
    }

}
