using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public AuthManager authManager;

    public void OnClickSignUp()
    {
        authManager.SignUp(emailInput.text, passwordInput.text);
    }

    public void OnClickSignIn()
    {
        authManager.SignIn(emailInput.text, passwordInput.text);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickSignOut()
    {
        authManager.SignOut();
    }
}

