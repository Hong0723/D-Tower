using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    private GoldBank bank;

    private void Awake()
    {
        bank = FindObjectOfType<GoldBank>();

        if (bank != null)
        {
            bank.OnGoldChanged += UpdateGoldText;
        }
    }

    private void Start()
    {
        if (bank != null)
            UpdateGoldText(bank.gold);
    }

    private void UpdateGoldText(int amount)
    {
        if (goldText == null)
            goldText = GetComponentInChildren<TMP_Text>();

        if (goldText != null)
            goldText.text = $"Gold: {amount}";
    }

    private void OnDestroy()
    {
        if (bank != null)
            bank.OnGoldChanged -= UpdateGoldText;
    }
}
