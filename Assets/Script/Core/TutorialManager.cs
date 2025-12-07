using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("튜토리얼 UI")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button skipBtn;
    [SerializeField] private RectTransform highlightArrow;

    [Header("강조할 UI들")]
    [SerializeField] private RectTransform goldUI;
    [SerializeField] private RectTransform hpUI;
    [SerializeField] private RectTransform scoreUI;
    [SerializeField] private RectTransform waveUI;
    [SerializeField] private RectTransform skillBtn;
    [SerializeField] private RectTransform towerSelectPanel;

    private int currentStep = 0;
    private string[] tutorialMessages = new string[]
    {
        "D-Tower에 오신 것을 환영합니다!\n적의 웨이브를 막아 기지를 지키세요!",
        "적을 잡으면 골드를 획득해요!\n골드로 타워와 스킬을 사용할 수 있어요.",
        "흰색 노드를 클릭해서 타워를 설치하세요!\n타워가 자동으로 적을 공격해요.",
        "타워를 클릭하면 업그레이드할 수 있어요!\n높은 단계일수록 더 강력해요.",
        "스킬로 적을 한 번에 처치할 수 있어요!\n번개, 빙결, 메테오, 통나무 스킬이 있어요.",
        "적을 놓치면 HP가 깎여요!\nHP가 0이 되면 패배해요.",
        "모든 웨이브를 막으면 승리!\n점수 = 처치 점수 + (남은 HP x 10)",
        "랭킹에서 다른 플레이어와 경쟁하세요!\n행운을 빕니다!"
    };

    private RectTransform[] highlightTargets;
    private Coroutine bounceCoroutine;

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialDone", 0) == 1)
        {
            tutorialPanel.SetActive(false);
            if (highlightArrow != null)
                highlightArrow.gameObject.SetActive(false);
            return;
        }

        highlightTargets = new RectTransform[]
        {
            null,              // 0: 환영 (강조 없음)
            goldUI,            // 1: 골드
            towerSelectPanel,  // 2: 타워 설치 (노드 대신 타워 패널)
            towerSelectPanel,  // 3: 업그레이드
            skillBtn,          // 4: 스킬
            hpUI,              // 5: HP
            waveUI,            // 6: 웨이브
            scoreUI            // 7: 점수
        };

        nextBtn.onClick.AddListener(NextStep);
        skipBtn.onClick.AddListener(SkipTutorial);

        Time.timeScale = 0f;
        tutorialPanel.SetActive(true);
        if (highlightArrow != null)
            highlightArrow.gameObject.SetActive(false);
        ShowStep(0);
    }

    private void ShowStep(int step)
    {
        currentStep = step;

        if (step >= tutorialMessages.Length)
        {
            EndTutorial();
            return;
        }

        tutorialText.text = tutorialMessages[step];

        // 강조 표시
        if (step < highlightTargets.Length && highlightTargets[step] != null)
        {
            HighlightTarget(highlightTargets[step]);
        }
        else
        {
            if (highlightArrow != null)
                highlightArrow.gameObject.SetActive(false);
        }

        // 마지막 단계면 버튼 텍스트 변경
        if (step == tutorialMessages.Length - 1)
        {
            nextBtn.GetComponentInChildren<TMP_Text>().text = "시작!";
        }
        else
        {
            nextBtn.GetComponentInChildren<TMP_Text>().text = "다음";
        }
    }

    private void HighlightTarget(RectTransform target)
    {
        if (highlightArrow != null && target != null)
        {
            highlightArrow.gameObject.SetActive(true);

            // 화살표를 타겟 위에 위치
            Vector3 targetPos = target.position;
            targetPos.y += target.sizeDelta.y / 2 + 5f;
            highlightArrow.position = targetPos;

            // 바운스 애니메이션 시작
            if (bounceCoroutine != null)
                StopCoroutine(bounceCoroutine);
            bounceCoroutine = StartCoroutine(BounceAnimation());
        }
    }

    private IEnumerator BounceAnimation()
    {
        Vector3 originalPos = highlightArrow.position;

        while (true)
        {
            float time = 0f;

            // 아래로
            while (time < 0.3f)
            {
                time += Time.unscaledDeltaTime;
                float t = time / 0.3f;
                highlightArrow.position = originalPos + Vector3.down * 15f * t;
                yield return null;
            }

            time = 0f;

            // 위로
            while (time < 0.3f)
            {
                time += Time.unscaledDeltaTime;
                float t = time / 0.3f;
                highlightArrow.position = originalPos + Vector3.down * 15f * (1f - t);
                yield return null;
            }
        }
    }

    private void NextStep()
    {
        ShowStep(currentStep + 1);
    }

    private void SkipTutorial()
    {
        EndTutorial();
    }

    private void EndTutorial()
    {
        if (bounceCoroutine != null)
            StopCoroutine(bounceCoroutine);

        PlayerPrefs.SetInt("TutorialDone", 1);
        PlayerPrefs.Save();

        tutorialPanel.SetActive(false);
        if (highlightArrow != null)
            highlightArrow.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}