using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    
    void Start()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        if (_textMeshProUGUI)
        {
            _textMeshProUGUI.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
