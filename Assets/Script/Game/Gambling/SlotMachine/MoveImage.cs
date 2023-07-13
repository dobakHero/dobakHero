using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MoveImage : MonoBehaviour
{
    [SerializeField] private List<Sprite> imageList;
    private Image _image;

    private bool _isMove = true;
    private bool _isSelected;

    private int _curImageIdx;

    private bool _isFirst;
    
    private const int Speed = 5;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = imageList[Random.Range(0, imageList.Count)];

        Debug.Log(transform.localPosition.y);
        
        if (transform.localPosition.y < 200f)
        {
            _isFirst = true;
        }
        else
        {
            _isFirst = false;
        }
        
        Debug.Log(_isFirst);
    }

    private void OnEnable()
    {
        _isMove = true;
        _isSelected = false;
        
        if (_isFirst)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x,0,localPosition.z);
            transform.localPosition = localPosition;
        }
        else
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x,300,localPosition.z);
            transform.localPosition = localPosition;
        }

        var rand = Random.Range(0, imageList.Count);
        if (_image)
        {
            _image.sprite = imageList[rand];
        }

        _curImageIdx = rand;
    }

    private void FixedUpdate()
    {
        if (_isMove)
        {
            transform.Translate(Vector3.down * (Speed * Time.deltaTime));
            if (transform.localPosition.y < -300)
            {
                var localPosition = transform.localPosition;
                localPosition = new Vector3(localPosition.x, 300, localPosition.z);
                transform.localPosition = localPosition;

                var rand = Random.Range(0, imageList.Count);
                _image.sprite = imageList[rand];
                _curImageIdx = rand;
            }
        }
        else
        {
            if (transform.localPosition.y is > -150 and < 150)
            {
                //Selected
                var localPosition = transform.localPosition;
                localPosition = new Vector3(localPosition.x,Mathf.Lerp(localPosition.y, 0, 0.05f),localPosition.z);
                transform.localPosition = localPosition;

                _isSelected = true;
            }
            else if (transform.localPosition.y > 150)
            {
                var localPosition = transform.localPosition;
                localPosition = new Vector3(localPosition.x,Mathf.Lerp(localPosition.y, 300, 0.05f),localPosition.z);
                transform.localPosition = localPosition;
            }
            else
            {
                var localPosition = transform.localPosition;
                localPosition = new Vector3(localPosition.x,Mathf.Lerp(localPosition.y, -300, 0.05f),localPosition.z);
                transform.localPosition = localPosition;
            }
        }
    }

    public void Stop()
    {
        _isMove = false;
    }

    public bool IsSelected()
    {
        return _isSelected;
    }

    public int GetCurImgIdx()
    {
        return _curImageIdx;
    }
}
