using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGambling : MonoBehaviour
{
    [SerializeField] private GameObject slotMachinePanel;
    [SerializeField] private GameObject horseRacingPanel;
    [SerializeField] private GameObject blackJackPanel;
    [SerializeField] private GameObject lotteryPanel;

    private void Start()
    {
        if (slotMachinePanel)
        {
            slotMachinePanel.SetActive(false);
        }

        if (horseRacingPanel)
        {
            horseRacingPanel.SetActive(false);
        }

        if (blackJackPanel)
        {
            blackJackPanel.SetActive(false);
        }

        if (lotteryPanel)
        {
            lotteryPanel.SetActive(false);
        }
    }

    public void OpenSlotMachinePanel()
    {
        if (GameManager.Instance.gamblingCurrentCount < 0)
            return;
        
        if (slotMachinePanel)
        {
            slotMachinePanel.SetActive(true);
        }
    }

    public void OpenHorseRacingPanel()
    {
        if (GameManager.Instance.gamblingCurrentCount < 0)
            return;
        
        if (horseRacingPanel)
        {
            horseRacingPanel.SetActive(true);
        }
    }

    public void OpenBlackJackPanel()
    {
        if (GameManager.Instance.gamblingCurrentCount < 0)
            return;
        
        if (blackJackPanel)
        {
            blackJackPanel.SetActive(true);
        }
    }

    public void OpenLotteryPanel()
    {
        if (GameManager.Instance.gamblingCurrentCount < 0)
            return;
        
        if (lotteryPanel)
        {
            lotteryPanel.SetActive(true);
        }
    }
}
