using UnityEngine;
using TMPro;

public class PlayerTreasureUI : MonoBehaviour
{
    public TreasureInventory inventory;
    public TextMeshProUGUI text;

    void Start()
    {
        text.text = $"Inventory: {inventory.treasures}/{inventory.maxTreasures}";
        inventory.OnTreasureChanged += UpdateUI;
    }

    void UpdateUI(int current, int max)
    {
        text.text = $"Inventory: {current}/{max}";
    }
}
