using UnityEngine;
using TMPro;
using System.Collections;

public class AlertUI : MonoBehaviour
{
    public static AlertUI Instance;

    [SerializeField] private TMP_Text alertText;
    [SerializeField] private float displayTime = 1.5f;

    private Coroutine currentAlert;

    private void Awake()
    {
        Instance = this;
        alertText.text = "";
    }

    public void ShowAlert(string message)
    {
        if (currentAlert != null)
            StopCoroutine(currentAlert);

        currentAlert = StartCoroutine(ShowAlertRoutine(message));
    }

    private IEnumerator ShowAlertRoutine(string message)
    {
        alertText.text = message;
        yield return new WaitForSeconds(displayTime);
        alertText.text = "";
        currentAlert = null;
    }
}