using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public void GetGold()
    {
        GameManager.Instance.Gold += 10000;
    }

    public void GetStress()
    {
        GameManager.Instance.Stress += 10;
    }

    public void ReduceStress()
    {
        GameManager.Instance.Stress -= 10;
    }

    public void GetChip()
    {
        GameManager.Instance.Chip += 100;
    }
}
