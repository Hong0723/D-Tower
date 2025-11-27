using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    private GoldBank bank;

    private void Start()
    {
        bank = FindObjectOfType<GoldBank>();
        if (bank != null)
        {
            bank.OnGoldChanged += UpdateGoldText;
            UpdateGoldText(bank.gold);
        }
    }

    private void UpdateGoldText(int amount)
    {
        goldText.text = $"Gold: {amount}";
    }

    private void OnDestroy()
    {
        if (bank != null)
            bank.OnGoldChanged -= UpdateGoldText;
    }
}