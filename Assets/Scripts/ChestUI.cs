using UnityEngine;
using TMPro;

public class ChestUI : MonoBehaviour
{
    public TreasureChest chest;
    public TextMeshProUGUI text;

    void Start()
    {
        text.text = $"Chest: {chest.storedTreasures}/{chest.requiredTreasures}";
        chest.OnChestChanged += UpdateUI;
    }

    void UpdateUI(int current, int max)
    {
        text.text = $"Chest: {current}/{max}";
    }
}