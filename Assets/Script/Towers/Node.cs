using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Node : MonoBehaviour
{
    public GameObject towerOnTop;
    public Color hoverColor = Color.green;
    private Color originColor;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originColor = sr.color;
    }

    void OnMouseEnter()
    {
        if (towerOnTop != null) return;

        if (sr != null) sr.color = hoverColor;
    }

    void OnMouseExit()
    {
        if (towerOnTop != null) return;

        if (sr != null) sr.color = originColor;
    }

    void OnMouseDown()
    {
        Debug.Log("Node 클릭됨!");

        TowerSelectUI ui = FindObjectOfType<TowerSelectUI>();
        if (ui == null)
        {
            Debug.Log("TowerSelectUI를 찾을 수 없음!");
            return;
        }

        if (towerOnTop == null)
        {
            Debug.Log("타워 없음 - ShowForNode 호출");
            ui.ShowForNode(this);
        }
        else
        {
            Tower tower = towerOnTop.GetComponent<Tower>();
            if (tower != null)
            {
                Debug.Log($"타워 있음 - 레벨 {tower.CurrentLevel}");
                ui.ShowForUpgrade(this, tower.CurrentLevel);
            }
        }
    }

    public void SetPlacedColor()
    {
        if (sr != null)
            sr.color = new Color32(0, 0, 0, 255);   // 검정
    }
}