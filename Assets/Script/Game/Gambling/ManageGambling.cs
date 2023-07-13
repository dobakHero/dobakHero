using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGambling : MonoBehaviour
{
    [SerializeField] private GameObject slotMachinePanel;
    [SerializeField] private GameObject horseRacingPanel;
    [SerializeField] private GameObject blackJackPanel;
    
    public void OpenSlotMachinePanel()
    {
        if (slotMachinePanel)
        {
            slotMachinePanel.SetActive(true);
        }
    }

    public void OpenHorseRacingPanel()
    {
        if (horseRacingPanel)
        {
            horseRacingPanel.SetActive(true);
        }
    }

    public void OpenBlackJackPanel()
    {
        if (blackJackPanel)
        {
            blackJackPanel.SetActive(true);
        }
    }
}
