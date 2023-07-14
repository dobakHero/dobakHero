using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageLottery : MonoBehaviour
{
    [SerializeField] private ManageLotteryBtn[] lotteryBtns;

    public void InactiveAllBtn()
    {
        foreach (var lotteryBtn in lotteryBtns)
        {
            lotteryBtn.InactiveUpDownBtn();
        }
    }

    public void BuyLottery()
    {
        if (GameManager.Instance.Chip < GameManager.Instance.GamblingList[4].Cost || GameManager.Instance.Day is EDay.Saturday or EDay.Sunday)
            return;

        var sum = 0;
        foreach (var lotteryBtn in lotteryBtns)
        {
            sum += lotteryBtn.GetLotteryNum();
            sum *= 10;
        }

        sum /= 10;

        foreach (var lottery in GameManager.Instance.lotteryList)
        {
            if (lottery == sum)
                return;
        }
        
        GameManager.Instance.Chip -= GameManager.Instance.GamblingList[4].Cost;
        GameManager.Instance.Stress += GameManager.Instance.GamblingList[4].Stress;
        
        GameManager.Instance.lotteryList.Add(sum);
    }
}
