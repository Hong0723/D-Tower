using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinSceneManager : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button rankingBtn;
    [SerializeField] private Button mainMenuBtn;

    private int finalScore;
    private int finalHP;
    private int totalScore;
    private string nickname;

    private void Start()
    {
        Time.timeScale = 1f;

        // 저장된 점수 불러오기
        finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        finalHP = PlayerPrefs.GetInt("FinalHP", 0);
        totalScore = finalScore + (finalHP * 10);
        nickname = AuthManager.GetNickname();

        // UI 표시
        if (scoreText != null)
            scoreText.text = $"처치 점수: {finalScore}";
        if (hpText != null)
            hpText.text = $"남은 HP: {finalHP} x 10 = {finalHP * 10}";
        if (totalScoreText != null)
            totalScoreText.text = $"총 점수: {totalScore}";
        if (nicknameText != null)
            nicknameText.text = $"플레이어: {nickname}";

        // 버튼 연결
        if (rankingBtn != null)
            rankingBtn.onClick.AddListener(OnRankingClick);
        if (mainMenuBtn != null)
            mainMenuBtn.onClick.AddListener(OnMainMenuClick);

        // 자동으로 랭킹 등록
        RegisterScore();
    }

    private async void RegisterScore()
    {
        if (RankingManager.Instance == null)
        {
            GameObject rm = new GameObject("RankingManager");
            rm.AddComponent<RankingManager>();
            await System.Threading.Tasks.Task.Delay(500);
        }

        bool success = await RankingManager.Instance.SaveScore(nickname, finalScore, finalHP);

        if (resultText != null)
        {
            if (success)
                resultText.text = "랭킹 등록 완료!";
            else
                resultText.text = "랭킹 등록 실패";
        }
    }

    private void OnRankingClick()
    {
        SceneManager.LoadScene("Ranking");
    }

    private void OnMainMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}