// MainMenuScene.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    public void GoPlayMap()
    {
        if (FirebaseInit.user == null)
        {
            Debug.Log("로그인이 필요합니다.");
            SceneManager.LoadScene("Login");  // 로그인 화면으로 이동
            return;
        }

        SceneManager.LoadScene("WaveSpawnScene");
    }

    public void GoLogin()
    {
        SceneManager.LoadScene("Login");
    }
}