using UnityEngine;

public class GoldBank : MonoBehaviour
{
    public int gold = 200;                      // Ω√¿€ ∞ÒµÂ
    public System.Action<int> OnGoldChanged;

    public bool Spend(int amount)
    {
        if (gold < amount) return false;
        gold -= amount;
        OnGoldChanged?.Invoke(gold);
        return true;
    }

    public void Earn(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }
}
