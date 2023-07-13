using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveHorse : MonoBehaviour
{
    [SerializeField] private ManageHorseRacing manageHorseRacing;
    
    private int _speed;
    private bool _isMove;

    private void OnEnable()
    {
        _isMove = false;
        _speed = 0;
        
        var localPosition = transform.localPosition;
        localPosition = new Vector3(localPosition.x, -40, localPosition.z);
        transform.localPosition = localPosition;
    }

    private void FixedUpdate()
    {
        if (_isMove)
        {
            transform.Translate(Vector3.down * (_speed * Time.deltaTime));

            if (transform.localPosition.y < -1000f)
            {
                _isMove = false;
                manageHorseRacing.FinishMove();
            }
        }
    }

    public void Move(bool isWin)
    {
        if (isWin)
        {
            _speed = 5;
        }
        else
        {
            _speed = Random.Range(1, 4);
        }

        _isMove = true;
    }
}
