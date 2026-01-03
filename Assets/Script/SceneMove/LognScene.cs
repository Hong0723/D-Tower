using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{
    void ResumeTime() => Time.timeScale = 1f;

    public void GoMainMenu()
    {
        ResumeTime();
        SceneManager.LoadScene("MainMenu");

    }
}
