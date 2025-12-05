using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [Header("이펙트 프리팹")]
    [SerializeField] private GameObject lightningEffectPrefab;

    [Header("스킬 설정 - 번개")]

    [SerializeField] private float lightningRadius = 2f;
    [SerializeField] private int lightningDamage = 30;
    [SerializeField] private float lightningCooldown = 10f;
    [SerializeField] private int lightningCost = 30;

    [Header("스킬 설정 - 빙결")]
    [SerializeField] private float freezeDuration = 3f;
    [SerializeField] private float freezeSlowPercent = 0.5f;
    [SerializeField] private float freezeCooldown = 15f;
    [SerializeField] private int freezeCost = 50;
    [SerializeField] private CanvasGroup freezeOverlayGroup;
    [SerializeField] private float freezeFadeInTime = 0.3f; 
    [SerializeField] private float freezeFadeOutTime = 0.5f;

    [Header("스킬 설정 - 메테오")]
    [SerializeField] private float meteorRadius = 2.5f;
    [SerializeField] private int meteorDamage = 80;
    [SerializeField] private float meteorCooldown = 20f;
    [SerializeField] private int meteorCost = 80;

    [Header("스킬 설정 - 통나무")]
    [SerializeField] private float logWidth = 1.5f;
    [SerializeField] private float logDistance = 10f;
    [SerializeField] private int logDamage = 20;
    [SerializeField] private float logKnockback = 2f;
    [SerializeField] private float logCooldown = 12f;
    [SerializeField] private int logCost = 40;
    [SerializeField] private Sprite logSprite;

    private Dictionary<string, float> cooldownTimers = new Dictionary<string, float>();
    private string selectedSkill = null;
    private bool isSelectingTarget = false;
    private GameObject currentIndicator = null;

    public System.Action<string, float, float> OnCooldownUpdate;

    private void Awake()
    {
        Instance = this;
        cooldownTimers["lightning"] = 0f;
        cooldownTimers["freeze"] = 0f;
        cooldownTimers["meteor"] = 0f;
        cooldownTimers["log"] = 0f;
    }

    private void Update()
    {
        UpdateCooldowns();

        if (isSelectingTarget)
        {
            UpdateIndicatorPosition();

            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return;

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 10f;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                mousePos.z = 0;

                UseSkillAtPosition(selectedSkill, mousePos);
                CancelSelection();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelSelection();
                if (AlertUI.Instance != null)
                    AlertUI.Instance.ShowAlert("Cancelled");
            }
        }
    }

    private void UpdateCooldowns()
    {
        List<string> keys = new List<string>(cooldownTimers.Keys);
        foreach (string key in keys)
        {
            if (cooldownTimers[key] > 0)
            {
                cooldownTimers[key] -= Time.deltaTime;
                float maxCooldown = GetMaxCooldown(key);
                OnCooldownUpdate?.Invoke(key, cooldownTimers[key], maxCooldown);
            }
        }
    }

    private float GetMaxCooldown(string skill)
    {
        switch (skill)
        {
            case "lightning": return lightningCooldown;
            case "freeze": return freezeCooldown;
            case "meteor": return meteorCooldown;
            case "log": return logCooldown;
            default: return 1f;
        }
    }

    public void SelectSkill(string skillName)
    {
        if (cooldownTimers[skillName] > 0)
        {
            if (AlertUI.Instance != null)
                AlertUI.Instance.ShowAlert("Cooldown!");
            return;
        }

        int cost = GetSkillCost(skillName);
        GoldBank bank = FindObjectOfType<GoldBank>();
        if (bank == null || bank.gold < cost)
        {
            if (AlertUI.Instance != null)
                AlertUI.Instance.ShowAlert("Not enough gold!");
            return;
        }

        if (skillName == "freeze")
        {
            bank.Spend(cost);
            UseFreeze();
        }
        else
        {
            selectedSkill = skillName;
            isSelectingTarget = true;
            CreateIndicator(skillName);
            if (AlertUI.Instance != null)
                AlertUI.Instance.ShowAlert("Click to use!");
        }
    }

    private void CreateIndicator(string skill)
    {
        if (currentIndicator != null)
            Destroy(currentIndicator);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        currentIndicator = new GameObject("SkillIndicator");
        SpriteRenderer sr = currentIndicator.AddComponent<SpriteRenderer>();

        if (skill == "log")
        {
            sr.sprite = CreateSquareSprite();
            sr.color = new Color(1f, 1f, 1f, 0.08f);
            currentIndicator.transform.localScale = new Vector3(2f, 0.8f, 1f);
        }
        else
        {
            sr.sprite = CreateCircleSprite();
            sr.color = new Color(1f, 1f, 1f, 0.08f);

            float radius = skill == "lightning" ? lightningRadius : meteorRadius;
            currentIndicator.transform.localScale = new Vector3(radius * 2, radius * 2, 1);
        }

        sr.sortingLayerName = "UI";
        sr.sortingOrder = 1000;
        currentIndicator.transform.position = mousePos;
    }

    private Sprite CreateCircleSprite()
    {
        int resolution = 64;
        Texture2D texture = new Texture2D(resolution, resolution);
        Color[] colors = new Color[resolution * resolution];

        float center = resolution / 2f;
        float radius = resolution / 2f;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (dist < radius)
                    colors[y * resolution + x] = Color.white;
                else
                    colors[y * resolution + x] = Color.clear;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f), resolution);
    }

    private Sprite CreateSquareSprite()
    {
        int resolution = 32;
        Texture2D texture = new Texture2D(resolution, resolution);
        Color[] colors = new Color[resolution * resolution];

        for (int i = 0; i < colors.Length; i++)
            colors[i] = Color.white;

        texture.SetPixels(colors);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f), resolution);
    }

    private void UpdateIndicatorPosition()
    {
        if (currentIndicator != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;
            currentIndicator.transform.position = mousePos;
        }
    }

    private void CancelSelection()
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
            currentIndicator = null;
        }
        isSelectingTarget = false;
        selectedSkill = null;
    }

    private int GetSkillCost(string skill)
    {
        switch (skill)
        {
            case "lightning": return lightningCost;
            case "freeze": return freezeCost;
            case "meteor": return meteorCost;
            case "log": return logCost;
            default: return 0;
        }
    }

    private void UseSkillAtPosition(string skill, Vector3 position)
    {
        GoldBank bank = FindObjectOfType<GoldBank>();
        int cost = GetSkillCost(skill);

        if (!bank.Spend(cost))
        {
            if (AlertUI.Instance != null)
                AlertUI.Instance.ShowAlert("Not enough gold!");
            return;
        }

        switch (skill)
        {
            case "lightning":
                UseLightning(position);
                break;
            case "meteor":
                UseMeteor(position);
                break;
            case "log":
                UseLog(position);
                break;
        }
    }

    private void UseLightning(Vector3 position)
    {
        cooldownTimers["lightning"] = lightningCooldown;

        if (lightningEffectPrefab != null)
        {
            GameObject effect = Instantiate(lightningEffectPrefab, position, Quaternion.identity);

            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            float duration = ps != null ? ps.main.duration : 1f;
            Destroy(effect, duration + 0.5f); 
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, lightningRadius);
        int hitCount = 0;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Health health = hit.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(lightningDamage);
                    hitCount++;
                }
            }
        }
        Debug.Log($"Lightning! {hitCount} enemies hit, {lightningDamage} damage");
    }

    private void UseFreeze()
    {
        cooldownTimers["freeze"] = freezeCooldown;

        if (freezeOverlayGroup != null)
        {
            StartCoroutine(PlayFreezeScreenEffect());
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            EnemyMover mover = enemy.GetComponent<EnemyMover>();
            if (mover != null)
            {
                StartCoroutine(FreezeEnemy(mover));
            }

            SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                StartCoroutine(FreezeColor(sr));
            }
        }
        Debug.Log($"Freeze! {enemies.Length} enemies slowed");
    }

    private IEnumerator PlayFreezeScreenEffect()
    {
        float timer = 0f;
        while (timer < freezeFadeInTime)
        {
            timer += Time.deltaTime;
            freezeOverlayGroup.alpha = Mathf.Lerp(0f, 1f, timer / freezeFadeInTime);
            yield return null;
        }
        freezeOverlayGroup.alpha = 1f; 

        float waitTime = freezeDuration - (freezeFadeInTime + freezeFadeOutTime);
        if (waitTime < 0) waitTime = 0.5f; 

        yield return new WaitForSeconds(waitTime);

        timer = 0f;
        while (timer < freezeFadeOutTime)
        {
            timer += Time.deltaTime;
            freezeOverlayGroup.alpha = Mathf.Lerp(1f, 0f, timer / freezeFadeOutTime);
            yield return null;
        }
        freezeOverlayGroup.alpha = 0f;
    }

    private IEnumerator FreezeEnemy(EnemyMover mover)
    {
        if (mover == null) yield break;

        mover.SetSpeedMultiplier(freezeSlowPercent);
        yield return new WaitForSeconds(freezeDuration);

        if (mover != null)
            mover.SetSpeedMultiplier(1f);
    }

    private IEnumerator FreezeColor(SpriteRenderer sr)
    {
        if (sr == null) yield break;

        Color originalColor = sr.color;
        sr.color = Color.cyan;
        yield return new WaitForSeconds(freezeDuration);

        if (sr != null)
            sr.color = originalColor;
    }

    private void UseMeteor(Vector3 position)
    {
        cooldownTimers["meteor"] = meteorCooldown;
        StartCoroutine(MeteorFall(position));
    }

    private IEnumerator MeteorFall(Vector3 position)
    {
        GameObject meteor = new GameObject("MeteorEffect");
        SpriteRenderer sr = meteor.AddComponent<SpriteRenderer>();
        sr.sprite = CreateCircleSprite();
        sr.color = new Color(1f, 0.3f, 0f, 1f);
        sr.sortingLayerName = "UI";
        sr.sortingOrder = 1000;

        Vector3 startPos = position + new Vector3(0, 8, 0);
        meteor.transform.position = startPos;
        meteor.transform.localScale = Vector3.one * 0.5f;

        float fallTime = 0.4f;
        float elapsed = 0f;

        while (elapsed < fallTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallTime;
            meteor.transform.position = Vector3.Lerp(startPos, position, t);
            meteor.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one * meteorRadius, t);
            yield return null;
        }

        sr.color = new Color(1f, 0.5f, 0f, 1f);
        meteor.transform.localScale = Vector3.one * meteorRadius * 2.5f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, meteorRadius);
        int hitCount = 0;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Health health = hit.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(meteorDamage);
                    hitCount++;
                }
            }
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(meteor);

        Debug.Log($"Meteor! {hitCount} enemies hit, {meteorDamage} damage");
    }

    private void UseLog(Vector3 position)
    {
        cooldownTimers["log"] = logCooldown;
        StartCoroutine(RollLog(position));
    }

    private IEnumerator RollLog(Vector3 startPos)
    {
        GameObject log = new GameObject("LogEffect");
        SpriteRenderer sr = log.AddComponent<SpriteRenderer>();
        if (logSprite != null)
        {
            sr.sprite = logSprite;
            sr.color = Color.white;
        }
        else
        {
            sr.sprite = CreateSquareSprite();
            sr.color = new Color(0.55f, 0.27f, 0.07f, 1f);
        }
        sr.sortingLayerName = "UI";
        sr.sortingOrder = 1000;
        log.transform.localScale = new Vector3(0.08f, 0.08f, 1f);
        log.transform.position = startPos;

        float traveled = 0f;
        float speed = 8f;
        Vector3 direction = Vector3.left;  // 왼쪽으로 굴러감
        Vector3 currentPos = startPos;

        HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

        while (traveled < logDistance)
        {
            float step = speed * Time.deltaTime;
            currentPos += direction * step;
            traveled += step;

            log.transform.position = currentPos;
            log.transform.Rotate(0, 0, 500 * Time.deltaTime);  // 반대로 회전

            Collider2D[] hits = Physics2D.OverlapCircleAll(currentPos, logWidth / 2f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy") && !hitEnemies.Contains(hit.gameObject))
                {
                    hitEnemies.Add(hit.gameObject);

                    Health health = hit.GetComponent<Health>();
                    if (health != null)
                    {
                        health.TakeDamage(logDamage);
                    }

                    EnemyMover mover = hit.GetComponent<EnemyMover>();
                    if (mover != null)
                    {
                        mover.Knockback(direction * logKnockback);  // 왼쪽으로 밀기
                    }
                }
            }
            yield return null;
        }

        Destroy(log);
        Debug.Log($"Log rolled! {hitEnemies.Count} enemies hit");
    }
}