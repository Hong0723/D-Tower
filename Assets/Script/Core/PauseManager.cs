using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button soundBtn;
    [SerializeField] private GameObject soundOnIcon;
    [SerializeField] private GameObject soundOffIcon;
    [SerializeField] private GameObject skillToggleBtn;
    [SerializeField] private GameObject skillPanel;

    private bool isPaused = false;
    private bool isSoundOn = true;

    private void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        // 저장된 사운드 설정 불러오기
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        UpdateSoundUI();
        ApplySound();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }
    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        // 스킬 버튼 숨기기
        if (skillToggleBtn != null)
            skillToggleBtn.SetActive(false);
        if (skillPanel != null)
            skillPanel.SetActive(false);
    }
    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        GameSpeedManager.I.ResumeGame();

        // 스킬 버튼 다시 보이기
        if (skillToggleBtn != null)
            skillToggleBtn.SetActive(true);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        Debug.Log("게임 종료!");
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSoundUI();
        ApplySound();
    }

    private void UpdateSoundUI()
    {
        if (soundOnIcon != null)
            soundOnIcon.SetActive(isSoundOn);
        if (soundOffIcon != null)
            soundOffIcon.SetActive(!isSoundOn);
    }

    private void ApplySound()
    {
        AudioListener.volume = isSoundOn ? 1f : 0f;
    }
}