using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("연결 스크립트")]
    [SerializeField] private WaveSpawner spawner;
    [SerializeField] private TMP_Text waveText;

    [Header("UI 설정")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject skipButton;

    [Header("웨이브 데이터 배열")]
    [SerializeField] private WaveData[] waves;

    [Header("웨이브 간 대기 시간")]
    [SerializeField] private float preStartDelay = 5f;
    [SerializeField] private float intermissionTime = 5f;

    [Header("배경음")]
    [SerializeField] private AudioClip normalBGM;
    [SerializeField] private AudioClip bossBGM;

    private int currentWave = 0;
    private bool isPreparing = false;
    private float timer = 0f;

    private Coroutine waveRoutine;

    private static readonly int[] bossWaves = { 10, 20, 30 }; // 🔥 보스 라운드 목록

    private void Start()
    {
        skipButton.SetActive(false);

        waveRoutine = StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (PlayerPrefs.GetInt("TutorialDone", 0) == 0)
            yield return null;

        // 첫 준비시간: 일반 BGM
        AudioManager.Instance?.PlayBGM(normalBGM);
        StartPrepare(preStartDelay);
        yield return WaitForPrepare();

        while (true)
        {
            if (currentWave >= waves.Length)
            {
                GameWin();
                yield break;
            }

            WaveData wave = waves[currentWave];
            currentWave++;

            // 🔥 웨이브 시작 시 UI 표시
            waveText.text = $"Wave {currentWave} 시작!";
            yield return new WaitForSeconds(1f);

            // 🔥 웨이브 전투 시작
            spawner.StartWave(wave.enemyPrefab, wave.enemyCount,
                              wave.spawnInterval, wave.speedMultiplier, wave.health);

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
                int nextWave = currentWave + 1;

                // 🔥 다음 웨이브가 보스라면 Boss BGM 재생
                if (System.Array.Exists(bossWaves, w => w == nextWave))
                    AudioManager.Instance?.PlayBGM(bossBGM);
                else
                    AudioManager.Instance?.PlayBGM(normalBGM);

                StartPrepare(intermissionTime);
                yield return WaitForPrepare();
            }
        }
    }

    private void StartPrepare(float time)
    {
        isPreparing = true;
        timer = time;
        skipButton.SetActive(true);
    }

    private IEnumerator WaitForPrepare()
    {
        while (timer > 0)
        {
            waveText.text = $"Wave {currentWave + 1} 준비... {Mathf.Ceil(timer)}";
            timer -= Time.deltaTime;
            yield return null;
        }

        EndPrepare();
    }

    private void EndPrepare()
    {
        isPreparing = false;
        skipButton.SetActive(false);
    }

    public void SkipPrepareTime()
    {
        if (isPreparing)
        {
            timer = 0f;
        }
    }

    private void GameWin()
    {
        waveText.text = "게임 승리!";
        int score = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
        int hp = FindObjectOfType<PlayerHP>() != null ? FindObjectOfType<PlayerHP>().CurrentHP : 0;

        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("FinalHP", hp);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene("Win");
    }
}
