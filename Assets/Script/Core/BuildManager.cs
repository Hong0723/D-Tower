using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager I;
    public GameObject selectedTower;
    public int selectedCost = 100;
    public int selectedLevel = 1;

    void Awake() { I = this; }

    public void SelectTower(GameObject prefab, int cost)
    {
        selectedTower = prefab;
        selectedCost = cost;
    }
}