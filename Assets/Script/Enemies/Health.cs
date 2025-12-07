using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHP = 40f;
    [SerializeField] private Healthbar healthbarPrefab;
    [SerializeField] private int goldReward = 5;
    [SerializeField] private int scoreReward = 10;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private int bossReward = 500; // 보스 처치 보상 점수/골드

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
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.monsterDie);

        ScoreManager.Instance?.AddScore(isBoss ? bossReward * 2 : scoreReward);

        GoldBank bank = FindObjectOfType<GoldBank>();
        if (bank != null)
        {
            bank.Earn(isBoss ? bossReward : goldReward);
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
