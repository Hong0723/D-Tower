using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int maxHP = 10;
    private int currentHP;

    public System.Action<int, int> OnHPChanged;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Start()
    {
        OnHPChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
        OnHPChanged?.Invoke(currentHP, maxHP);
        Debug.Log($"플레이어 HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("게임 오버!");
        SceneManager.LoadScene("Defeat");
    }
}