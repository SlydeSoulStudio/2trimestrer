using UnityEngine;
using System;

public class TreasureInventory : MonoBehaviour
{
    public int treasures = 0;
    public int maxTreasures = 3;

    // Evento para avisar a la UI
    public Action<int, int> OnTreasureChanged;

    public bool CanCarryMore()
    {
        return treasures < maxTreasures;
    }

    public void AddTreasure()
    {
        treasures++;
        OnTreasureChanged?.Invoke(treasures, maxTreasures);
    }

    public int DepositAll()
    {
        int amount = treasures;
        treasures = 0;

        OnTreasureChanged?.Invoke(treasures, maxTreasures);

        return amount;
    }
}