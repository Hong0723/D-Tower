using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatScene : MonoBehaviour
{
    // 패배 화면에서 Time.timeScale=0 했다면 로드 전에 꼭 1로
    void ResumeTime() => Time.timeScale = 1f;

    public void GoPlayMap()
    {
        ResumeTime();
        SceneManager.LoadScene("WaveSpawnScene");  // 정확한 씬 이름
    }

    public void GoMainMenu()
    {
        ResumeTime();
        SceneManager.LoadScene("MainMenu");   // 정확한 씬 이름
    }

    public void GoRankingMenu()
    {
        ResumeTime();
        SceneManager.LoadScene("Ranking");   // 정확한 씬 이름
    }
}
