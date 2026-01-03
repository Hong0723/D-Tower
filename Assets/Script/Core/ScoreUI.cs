using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
            UpdateScoreText(0);
        }
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreText;
    }
}