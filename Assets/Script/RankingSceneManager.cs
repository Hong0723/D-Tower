using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class RankData
{
    public string name;
    public int score;
}

[System.Serializable]
public class RankDataWrapper
{
    public List<RankData> rankList = new List<RankData>();
}

public class RankingSceneManager : MonoBehaviour
{
    [Header("UI 연결")]
    public InputField nameInput;       // 이름 입력창
    public Button registerButton;      // 등록 버튼
    public GameObject inputPanel;      // 이름 입력하는 패널

    [Header("랭킹 리스트 UI")]
    public Transform contentArea;     
    public GameObject rankItemPrefab;  
    public Text currentScoreText;     

    private int receivedScore = 0;
    private RankDataWrapper rankDataWrapper = new RankDataWrapper();

    void Start()
    {
        LoadRanking();

        receivedScore = PlayerPrefs.GetInt("TempScore", 0);

        if (receivedScore > 0)
        {
            inputPanel.SetActive(true);
            currentScoreText.text = $"당신의 점수: {receivedScore}";
            PlayerPrefs.SetInt("TempScore", 0);
        }
        else
        {
            inputPanel.SetActive(false); 
            currentScoreText.text = "랭킹 조회 모드";
        }
        UpdateRankingUI();
    }

    public void OnRegisterButton()
    {
        if (string.IsNullOrEmpty(nameInput.text)) return; 

        RankData newRank = new RankData();
        newRank.name = nameInput.text;
        newRank.score = receivedScore;

        rankDataWrapper.rankList.Add(newRank);

        rankDataWrapper.rankList = rankDataWrapper.rankList.OrderByDescending(x => x.score).ToList();

        if (rankDataWrapper.rankList.Count > 10)
            rankDataWrapper.rankList.RemoveAt(rankDataWrapper.rankList.Count - 1);

        SaveRanking();
        UpdateRankingUI();

        inputPanel.SetActive(false);
    }

    // 메인 화면으로 돌아가기 버튼용
    public void GoToMain()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

    void SaveRanking()
    {
        string json = JsonUtility.ToJson(rankDataWrapper);
        PlayerPrefs.SetString("GlobalRanking", json);
        PlayerPrefs.Save();
    }

    void LoadRanking()
    {
        string json = PlayerPrefs.GetString("GlobalRanking", "");
        if (!string.IsNullOrEmpty(json))
        {
            rankDataWrapper = JsonUtility.FromJson<RankDataWrapper>(json);
        }
    }

    void UpdateRankingUI()
    {
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < rankDataWrapper.rankList.Count; i++)
        {
            GameObject item = Instantiate(rankItemPrefab, contentArea);

            Text[] texts = item.GetComponentsInChildren<Text>();
            if (texts.Length >= 3)
            {
                texts[0].text = (i + 1).ToString() + "위"; // 등수
                texts[1].text = rankDataWrapper.rankList[i].name; // 이름
                texts[2].text = rankDataWrapper.rankList[i].score.ToString("N0"); // 점수 
            }
        }
    }
}
