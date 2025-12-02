using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class RankingSceneManager : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private Transform contentArea;
    [SerializeField] private GameObject rankItemPrefab;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private TMP_Text loadingText;

    private void Start()
    {
        if (mainMenuBtn != null)
            mainMenuBtn.onClick.AddListener(OnMainMenuClick);

        LoadRankings();
    }

    private async void LoadRankings()
    {
        if (loadingText != null)
            loadingText.text = "Loading...";

        // RankingManager 찾기
        if (RankingManager.Instance == null)
        {
            GameObject rm = new GameObject("RankingManager");
            rm.AddComponent<RankingManager>();
        }

        // 잠시 대기 (Firebase 초기화)
        await System.Threading.Tasks.Task.Delay(500);

        var rankings = await RankingManager.Instance.GetTopRankings(10);

        if (loadingText != null)
            loadingText.gameObject.SetActive(false);

        DisplayRankings(rankings);
    }

    private void DisplayRankings(List<RankingManager.RankingData> rankings)
    {
        // 기존 항목 삭제
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        // 시작 Y 위치
        float startY = 200f;
        float spacing = 70f;

        // 랭킹 표시
        for (int i = 0; i < rankings.Count; i++)
        {
            var data = rankings[i];
            GameObject item = Instantiate(rankItemPrefab, contentArea);

            // 위치 조정 (위에서 아래로)
            RectTransform rt = item.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, startY - (i * spacing));

            TMP_Text[] texts = item.GetComponentsInChildren<TMP_Text>();
            if (texts.Length >= 3)
            {
                texts[0].text = (i + 1).ToString();       // 순위
                texts[1].text = data.playerName;           // 이름
                texts[2].text = data.totalScore.ToString(); // 점수
            }

            // 1~3등 색상
            if (texts.Length >= 3)
            {
                if (i == 0)
                    texts[2].color = Color.red;
                else if (i == 1)
                    texts[2].color = Color.yellow;
                else if (i == 2)
                    texts[2].color = Color.green;
                else
                    texts[2].color = Color.white;
            }
        }

        Debug.Log($"랭킹 {rankings.Count}개 표시 완료");
    }

    private void OnMainMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}