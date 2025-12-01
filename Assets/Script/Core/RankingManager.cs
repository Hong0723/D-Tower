using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    private DatabaseReference dbReference;

    [System.Serializable]
    public class RankingData
    {
        public string playerName;
        public int score;
        public int hp;
        public int totalScore;

        public RankingData(string name, int score, int hp)
        {
            this.playerName = name;
            this.score = score;
            this.hp = hp;
            this.totalScore = score + (hp * 10);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeFirebase()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Firebase Database 초기화 완료");
    }

    public async Task<bool> SaveScore(string playerName, int score, int hp)
    {
        if (dbReference == null)
        {
            Debug.LogError("Firebase Database 연결 안됨");
            return false;
        }

        try
        {
            int newTotalScore = score + (hp * 10);

            // 기존 점수 확인
            var snapshot = await dbReference.Child("rankings")
                .OrderByChild("playerName")
                .EqualTo(playerName)
                .GetValueAsync();

            string existingKey = null;
            int existingScore = 0;

            if (snapshot.Exists)
            {
                foreach (var child in snapshot.Children)
                {
                    string json = child.GetRawJsonValue();
                    RankingData existingData = JsonUtility.FromJson<RankingData>(json);
                    if (existingData.playerName == playerName)
                    {
                        existingKey = child.Key;
                        existingScore = existingData.totalScore;
                        break;
                    }
                }
            }

            // 새 점수가 더 높으면 갱신
            if (existingKey != null)
            {
                if (newTotalScore > existingScore)
                {
                    RankingData data = new RankingData(playerName, score, hp);
                    string json = JsonUtility.ToJson(data);
                    await dbReference.Child("rankings").Child(existingKey).SetRawJsonValueAsync(json);
                    Debug.Log($"랭킹 갱신: {playerName} - {newTotalScore}점 (기존: {existingScore}점)");
                }
                else
                {
                    Debug.Log($"기존 점수가 더 높음: {existingScore}점 > {newTotalScore}점");
                }
            }
            else
            {
                // 새로 등록
                RankingData data = new RankingData(playerName, score, hp);
                string json = JsonUtility.ToJson(data);
                string key = dbReference.Child("rankings").Push().Key;
                await dbReference.Child("rankings").Child(key).SetRawJsonValueAsync(json);
                Debug.Log($"랭킹 등록: {playerName} - {newTotalScore}점");
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"랭킹 저장 실패: {e.Message}");
            return false;
        }
    }

    public async Task<List<RankingData>> GetTopRankings(int count = 10)
    {
        List<RankingData> rankings = new List<RankingData>();

        if (dbReference == null)
        {
            Debug.LogError("Firebase Database 연결 안됨");
            return rankings;
        }

        try
        {
            var snapshot = await dbReference.Child("rankings").GetValueAsync();

            if (snapshot.Exists)
            {
                foreach (var child in snapshot.Children)
                {
                    string json = child.GetRawJsonValue();
                    RankingData data = JsonUtility.FromJson<RankingData>(json);
                    rankings.Add(data);
                }

                rankings.Sort((a, b) => b.totalScore.CompareTo(a.totalScore));

                if (rankings.Count > count)
                    rankings = rankings.GetRange(0, count);
            }

            Debug.Log($"랭킹 불러오기 성공: {rankings.Count}개");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"랭킹 불러오기 실패: {e.Message}");
        }

        return rankings;
    }
}