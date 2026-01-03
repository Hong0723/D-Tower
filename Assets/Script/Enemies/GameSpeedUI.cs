using UnityEngine;
using TMPro;

public class GameSpeedUI : MonoBehaviour
{
    [SerializeField] private GameSpeedManager gsm;
    [SerializeField] private TMP_Text buttonText;

    private void Awake()
    {
        if (gsm == null)
            gsm = GameSpeedManager.I; // 자동 연결 (예외 방지)
    }

    private void OnEnable()
    {
        gsm.OnSpeedChanged += UpdateUI;
    }

    private void OnDisable()
    {
        gsm.OnSpeedChanged -= UpdateUI;
    }

    private void Start()
    {
        UpdateUI(gsm.CurrentSpeed); // UI 초기화
    }

    public void OnSpeedButtonClicked()
    {
        gsm.ToggleSpeed();
    }

    private void UpdateUI(float speed)
    {
        if (buttonText != null)
            buttonText.text = $"{speed}x";
    }
}
