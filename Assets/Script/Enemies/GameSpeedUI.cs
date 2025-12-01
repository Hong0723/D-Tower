using UnityEngine;
using TMPro;

public class GameSpeedUI : MonoBehaviour
{
    [SerializeField] private GameSpeedManager gsm;
    [SerializeField] private TMP_Text buttonText;

    void Start()
    {
        UpdateUI();
    }

    public void OnSpeedButtonClicked()
    {
        gsm.ToggleSpeed();
        UpdateUI();
    }

    private void UpdateUI()
    {
        buttonText.text = $"{gsm.CurrentSpeed}x";
    }
}
