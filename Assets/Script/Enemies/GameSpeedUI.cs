using UnityEngine;
using TMPro;

public class GameSpeedUI : MonoBehaviour
{
    [SerializeField] private GameSpeedManager gsm;
    [SerializeField] private TMP_Text buttonText;

    private void OnEnable()
    {
        gsm.OnSpeedChanged += UpdateUI;
    }

    private void OnDisable()
    {
        gsm.OnSpeedChanged -= UpdateUI;
    }

    void Start()
    {
        UpdateUI(gsm.CurrentSpeed);
    }

    public void OnSpeedButtonClicked()
    {
        gsm.ToggleSpeed();
    }

    private void UpdateUI(float speed)
    {
        buttonText.text = $"{speed}x";
    }
}
