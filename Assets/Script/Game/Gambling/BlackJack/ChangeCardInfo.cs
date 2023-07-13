using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCardInfo : MonoBehaviour
{
    [SerializeField] private Image mark;
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private Sprite[] marks;

    public void ChangeInfo(int n, EMark m)
    {
        mark.sprite = m switch
        {
            EMark.Spade => marks[0],
            EMark.Heart => marks[1],
            EMark.Clover => marks[2],
            EMark.Diamond => marks[3],
            _ => mark.sprite
        };

        number.text = n.ToString();
    }
}
