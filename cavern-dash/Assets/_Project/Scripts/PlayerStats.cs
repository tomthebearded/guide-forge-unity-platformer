using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int CoinsCollected { get; private set; }

    public event Action<int> CoinsChanged;

    public void AddCoin()
    {
        CoinsCollected++;

        CoinsChanged?.Invoke(CoinsCollected);

        Debug.Log($"coins = {CoinsCollected}");
    }
}
