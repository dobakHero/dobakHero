using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageRoulette : MonoBehaviour
{
    [SerializeField] private MoveRoulette[] roulettes;

    private const int SpeedRange = 10;

    public void RouletteStart()
    {
        
        
        var rand = Random.Range(0, SpeedRange);
        
        foreach (var roulette in roulettes)
        {
            roulette.AddSpeed(rand);
            roulette.MoveStart();
        }
    }
}
