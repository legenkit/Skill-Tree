using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance { get; set;}

    [Header("Data")]
    public int XP = 5;
    public int coin = 1965487;

    [Header("Reference")]
    [SerializeField] TextMeshProUGUI XPText;
    [SerializeField] TextMeshProUGUI CoinText;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AddXP(0);
        AddCoin(0);
    }

    public void AddXP(int xp = 1)
    {
        XP+= xp;
        XPText.SetText($"{XP}");
    }
    public void AddCoin(int xp = 1)
    {
        coin+= xp;
        CoinText.SetText($"{coin}");
    }
}
