using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("연결 스크립트")]
    [SerializeField] private WaveSpawner spawner;
    [SerializeField] private TMP_Text waveText;

    [Header("UI설정")]
    [SerializeField] private GameObject winPanel;

    [Header("웨이브 데이터 배열")]
    [SerializeField] private WaveData[] waves;

    [Header("웨이브 간 대기 시간")]
    [SerializeField] private float preStartDelay = 5f;
    [SerializeField] private float intermissionTime = 5f;

    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        // 튜토리얼 끝날 때까지 대기
        while (PlayerPrefs.GetInt("TutorialDone", 0) == 0)
        {
            yield return null;
        }

        float timer = preStartDelay;
        while (timer > 0)
        {
            waveText.text = $"Wave 1 준비... {Mathf.Ceil(timer)}";
            timer -= Time.deltaTime;
            yield return null;
        }

        while (true)
        {
            if (currentWave >= waves.Length)
            {
                GameWin();
                yield break;
            }

            var wave = waves[currentWave];
            currentWave++;

            waveText.text = $"Wave {currentWave} 시작!";
            yield return new WaitForSeconds(1f);

            spawner.StartWave(
                wave.enemyPrefab,
                wave.enemyCount,
                wave.spawnInterval,
                wave.speedMultiplier,
                wave.health
            );

            timer = wave.duration;
            while (timer > 0)
            {
                waveText.text = $"Wave {currentWave} 진행... {Mathf.Ceil(timer)}";
                timer -= Time.deltaTime;
                yield return null;
            }

            spawner.StopWave();
            waveText.text = $"Wave {currentWave} 종료!";
            yield return new WaitForSeconds(1f);

            if (currentWave < waves.Length)
            {
                timer = intermissionTime;
                while (timer > 0)
                {
                    waveText.text = $"Wave {currentWave + 1} 준비... {Mathf.Ceil(timer)}";
                    timer -= Time.deltaTime;
                    yield return null;
                }
            }
        }
    }

    private void GameWin()
    {
        Debug.Log("게임 클리어!");
        waveText.text = "게임 승리!";

        // 점수와 HP 저장
        int score = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
        int hp = FindObjectOfType<PlayerHP>() != null ? FindObjectOfType<PlayerHP>().CurrentHP : 0;

        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("FinalHP", hp);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene("Win");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
