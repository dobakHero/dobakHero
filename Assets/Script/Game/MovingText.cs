using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MovingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    void Start()
    {
        textMeshProUGUI.text = GameManager.Instance.ScriptList["Script_Start"];
        
        transform.DOMoveY(3, 10);
    }
}
