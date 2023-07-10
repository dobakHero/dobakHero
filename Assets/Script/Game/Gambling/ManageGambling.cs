using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGambling : MonoBehaviour
{
    [SerializeField] private GameObject slotMachinePanel;

    public void OpenSlotMachinePanel()
    {
        slotMachinePanel.SetActive(true);
    }
}
