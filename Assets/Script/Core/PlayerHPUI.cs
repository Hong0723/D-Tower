using UnityEngine;
using TMPro;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    private PlayerHP playerHP;

    private void Start()
    {
        playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP != null)
        {
            playerHP.OnHPChanged += UpdateHPText;
            UpdateHPText(playerHP.CurrentHP, playerHP.MaxHP);
        }
    }

    private void UpdateHPText(int current, int max)
    {
        hpText.text = $"HP: {current}/{max}";
    }

    private void OnDestroy()
    {
        if (playerHP != null)
            playerHP.OnHPChanged -= UpdateHPText;
    }
}