using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManageChipShop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chipText;
    [SerializeField] private TextMeshProUGUI chipToGoldText;
    [SerializeField] private TextMeshProUGUI goldToChipText;

    private int _chipCount;

    private const int GoldPerChip = 1000;
    private const int ChipPerGold = 800;

    private void OnEnable()
    {
        _chipCount = 1;
        chipText.text = _chipCount.ToString();

        chipToGoldText.text = (_chipCount * ChipPerGold).ToString();
        goldToChipText.text = (_chipCount * GoldPerChip).ToString();
    }

    public void IncreaseChip()
    {
        _chipCount += 1;
        chipText.text = _chipCount.ToString();

        chipToGoldText.text = (_chipCount * ChipPerGold).ToString();
        goldToChipText.text = (_chipCount * GoldPerChip).ToString();
    }

    public void DecreaseChip()
    {
        if (_chipCount - 1 <= 0)
            return;
        
        _chipCount -= 1;
        chipText.text = _chipCount.ToString();

        chipToGoldText.text = (_chipCount * ChipPerGold).ToString();
        goldToChipText.text = (_chipCount * GoldPerChip).ToString();
    }
    
    public void BuyChip()
    {
        if (GameManager.Instance.Gold < (_chipCount * GoldPerChip))
            return;

        GameManager.Instance.Gold -= (_chipCount * GoldPerChip);
        GameManager.Instance.Chip += _chipCount;

        GameManager.Instance.buyChipCount += _chipCount;
    }

    public void SellChip()
    {
        if (GameManager.Instance.Chip < _chipCount)
            return;

        GameManager.Instance.Gold += (_chipCount * ChipPerGold);
        GameManager.Instance.Chip -= _chipCount;
    }
}
