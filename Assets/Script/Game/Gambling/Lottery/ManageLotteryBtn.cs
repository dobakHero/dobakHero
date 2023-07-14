using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageLotteryBtn : MonoBehaviour
{
    [SerializeField] private Button upBtn;
    [SerializeField] private Button downBtn;
    [SerializeField] private TextMeshProUGUI lotteryText;

    private int _lotteryNum;

    private void OnEnable()
    {
        upBtn.gameObject.SetActive(false);
        downBtn.gameObject.SetActive(false);

        _lotteryNum = 0;
        lotteryText.text = _lotteryNum.ToString();
    }

    public void ActiveUpDownBtn()
    {
        upBtn.gameObject.SetActive(true);
        downBtn.gameObject.SetActive(true);
    }

    public void InactiveUpDownBtn()
    {
        upBtn.gameObject.SetActive(false);
        downBtn.gameObject.SetActive(false);
    }

    public void UpLotteryNum()
    {
        _lotteryNum++;
        if (_lotteryNum == 10)
        {
            _lotteryNum = 0;
        }
        
        lotteryText.text = _lotteryNum.ToString();
    }

    public void DownLotteryNum()
    {
        _lotteryNum--;
        if (_lotteryNum == -1)
        {
            _lotteryNum = 9;
        }

        lotteryText.text = _lotteryNum.ToString();
    }

    public int GetLotteryNum()
    {
        return _lotteryNum;
    }
}
