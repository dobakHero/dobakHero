using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MoveRoulette : MonoBehaviour
{
    [SerializeField] private Sprite[] rouletteImages;
    [SerializeField] private int order;

    private int _curIdx;
    private float _speed;

    private bool _isMove;
    private bool _isSelected;
    private bool _isStart;

    private const int MinSpeed = 32;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _image.sprite = rouletteImages[order];
        transform.localPosition = new Vector3(order * 300, 0, 0);

        _curIdx = order;
        
        _isMove = false;
        _isSelected = false;
        _isStart = false;
        
        _speed = MinSpeed;
    }

    private void FixedUpdate()
    {
        if(_isStart)
        {
            if (_isMove)
            {
                transform.Translate(Vector3.left * (_speed * Time.deltaTime));
                if (transform.localPosition.x < -300)
                {
                    var localPosition = transform.localPosition;
                    localPosition = new Vector3(300, localPosition.y, localPosition.z);
                    transform.localPosition = localPosition;

                    _curIdx += 2;
                    if (_curIdx >= rouletteImages.Length)
                    {
                        _curIdx %= rouletteImages.Length;
                    }

                    _image.sprite = rouletteImages[_curIdx];
                }

                if (_speed > 1)
                    _speed -= 0.1f;
                else
                {
                    _isMove = false;
                }
            }
            else
            {
                if (transform.localPosition.x >= 0 && transform.localPosition.x < 300)
                {
                    //Selected
                    var localPosition = transform.localPosition;
                    localPosition = new Vector3(Mathf.Lerp(localPosition.x, 0, 0.05f), localPosition.y,
                        localPosition.z);
                    transform.localPosition = localPosition;

                    _isSelected = true;
                }
                else
                {
                    var localPosition = transform.localPosition;
                    localPosition = new Vector3(Mathf.Lerp(localPosition.x, -300, 0.05f), localPosition.y,
                        localPosition.z);
                    transform.localPosition = localPosition;
                }
            }
        }
    }

    public void MoveStart()
    {
        _isStart = true;
        _isMove = true;
    }

    public void AddSpeed(int speed)
    {
        _speed += speed;
    }

    public bool IsSelected()
    {
        return _isSelected;
    }
}
