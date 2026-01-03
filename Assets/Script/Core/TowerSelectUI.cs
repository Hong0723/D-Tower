using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerSelectUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button[] towerButtons;
    [SerializeField] private TMP_Text[] buttonTexts;

    [Header("타워 설정")]
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private int[] towerCosts = { 10, 50, 100, 300, 500 };

    private Node currentNode;
    private bool isUpgradeMode = false;
    private bool justOpened = false;

    private void Start()
    {
        panel.SetActive(false);

        for (int i = 0; i < towerButtons.Length; i++)
        {
            int level = i + 1;
            towerButtons[i].onClick.AddListener(() => OnTowerButtonClicked(level));
        }
    }

    public void ShowForNode(Node node)
    {
        currentNode = node;
        isUpgradeMode = false;
        panel.SetActive(true);
        justOpened = true;

        for (int i = 0; i < towerButtons.Length; i++)
        {
            towerButtons[i].interactable = true;
            buttonTexts[i].text = $"{i + 1}단계\n{towerCosts[i]}G";
        }
    }

    public void ShowForUpgrade(Node node, int currentLevel)
    {
        currentNode = node;
        isUpgradeMode = true;
        panel.SetActive(true);
        justOpened = true;

        for (int i = 0; i < towerButtons.Length; i++)
        {
            int level = i + 1;
            if (level <= currentLevel)
            {
                towerButtons[i].interactable = false;
                buttonTexts[i].text = $"{level}단계\n설치됨";
            }
            else
            {
                towerButtons[i].interactable = true;
                buttonTexts[i].text = $"{level}단계\n{towerCosts[i]}G";
            }
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
        currentNode = null;
    }

    private void OnTowerButtonClicked(int level)
    {
        if (currentNode == null) return;

        int cost = towerCosts[level - 1];
        GoldBank bank = FindObjectOfType<GoldBank>();

        if (bank == null || !bank.Spend(cost))
        {
            if (AlertUI.Instance != null)
                AlertUI.Instance.ShowAlert("골드가 부족합니다!");
            return;
        }

        if (isUpgradeMode)
        {
            if (currentNode.towerOnTop != null)
            {
                Destroy(currentNode.towerOnTop);
            }
        }

        GameObject newTower = Instantiate(towerPrefab, currentNode.transform.position, Quaternion.identity);
        Tower tower = newTower.GetComponent<Tower>();
        if (tower != null)
        {
            tower.SetLevel(level);
        }
        currentNode.towerOnTop = newTower;
        currentNode.SetPlacedColor();
        Debug.Log($"{level}단계 타워 설치! -{cost}G");
        Hide();
    }

    private void LateUpdate()
    {
        if (justOpened)
        {
            justOpened = false;
            return;
        }

        if (panel.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUI())
            {
                Hide();
            }
        }
    }

    private bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}