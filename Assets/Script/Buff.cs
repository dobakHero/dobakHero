using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public Buff(int duration, bool isActive)
    {
        Duration = duration;
        IsActive = isActive;
    }
    
    public int Duration;
    public bool IsActive;
}
