using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MovingText : MonoBehaviour
{
    private Transform _transform;
    
    void Start()
    {
        _transform = GetComponent<Transform>();

        if (_transform)
        {
            _transform.DOMoveY(558, 10, true);
        }
    }
}
