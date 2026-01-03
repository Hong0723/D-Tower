using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    [Header("패널")]
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private Button skillToggleBtn;

    [Header("스킬 버튼")]
    [SerializeField] private Button lightningBtn;
    [SerializeField] private Button freezeBtn;
    [SerializeField] private Button meteorBtn;
    [SerializeField] private Button logBtn;

    [Header("쿨타임 텍스트")]
    [SerializeField] private TMP_Text lightningCooldownText;
    [SerializeField] private TMP_Text freezeCooldownText;
    [SerializeField] private TMP_Text meteorCooldownText;
    [SerializeField] private TMP_Text logCooldownText;

    private void Start()
    {
        skillPanel.SetActive(false);

        skillToggleBtn.onClick.AddListener(ToggleSkillPanel);

        lightningBtn.onClick.AddListener(() => UseSkill("lightning"));
        freezeBtn.onClick.AddListener(() => UseSkill("freeze"));
        meteorBtn.onClick.AddListener(() => UseSkill("meteor"));
        logBtn.onClick.AddListener(() => UseSkill("log"));

        if (SkillManager.Instance != null)
            SkillManager.Instance.OnCooldownUpdate += UpdateCooldownUI;
    }

    private void ToggleSkillPanel()
    {
        skillPanel.SetActive(!skillPanel.activeSelf);
    }

    private void UseSkill(string skillName)
    {
        if (SkillManager.Instance != null)
        {
            SkillManager.Instance.SelectSkill(skillName);
            skillPanel.SetActive(false);
        }
    }

    private void UpdateCooldownUI(string skill, float remaining, float max)
    {
        TMP_Text text = null;
        switch (skill)
        {
            case "lightning": text = lightningCooldownText; break;
            case "freeze": text = freezeCooldownText; break;
            case "meteor": text = meteorCooldownText; break;
            case "log": text = logCooldownText; break;
        }

        if (text != null)
        {
            if (remaining > 0)
                text.text = Mathf.Ceil(remaining).ToString() + "s";
            else
                text.text = "";
        }
    }

    private void OnDestroy()
    {
        if (SkillManager.Instance != null)
            SkillManager.Instance.OnCooldownUpdate -= UpdateCooldownUI;
    }
}