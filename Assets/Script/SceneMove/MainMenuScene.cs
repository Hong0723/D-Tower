using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene: MonoBehaviour
{
    public void GoPlayMap()
    {
        SceneManager.LoadScene("WaveSpawnScene");  // Map 씬 이름과 동일하게
    }

    public void GoLogin()
    {
        SceneManager.LoadScene("Login");  // Map 씬 이름과 동일하게
    }
}
