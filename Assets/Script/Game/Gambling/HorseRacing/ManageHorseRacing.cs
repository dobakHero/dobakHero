using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ManageHorseRacing : MonoBehaviour
{
    [SerializeField] private GameObject[] horses;
    [SerializeField] private GameObject[] winRateTexts;
    [SerializeField] private GameObject chipGameObject;
    [SerializeField] private GameObject[] selectButtons;
    [SerializeField] private GameObject exitButton;
    private Image[] _buttonImages;
    private TextMeshProUGUI _chipText;
    private Button[] _buttons;

    private int[] _horseWinRate;
    private float[] _horseDividend;

    private int _chipCount;

    private int _selectHorseIdx;

    private const int HorseNumber = 4;
    private const float ChangeAlphaValue = 0.3f;
    private void Awake()
    {
        _chipText = chipGameObject.GetComponent<TextMeshProUGUI>();
        
        _horseWinRate = new int[HorseNumber];
        _horseDividend = new float[HorseNumber];
        _buttonImages = new Image[HorseNumber];

        for (var i = 0; i < HorseNumber; i++)
        {
            _buttonImages[i] = selectButtons[i].GetComponent<Image>();
        }

        _buttons = GetComponentsInChildren<Button>();
    }

    private void OnEnable()
    {
        _chipCount = 10;
        _chipText.text = _chipCount.ToString();

        _selectHorseIdx = -1;

        foreach (var buttonImage in _buttonImages)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
        }

        foreach (var button in _buttons)
        {
            button.interactable = true;
        }
        
        foreach (var horse in horses)
        {
            var localPosition = horse.transform.localPosition;
            localPosition = new Vector3(localPosition.x, -40, localPosition.z);
            horse.transform.localPosition = localPosition;
        }

        int[] arr = { 0, 1, 2, 3 };

        ShuffleArray(arr);

        for (var i = 0; i < HorseNumber; i++)
        {
            var winRate = 0;
            var dividend = 0f;

            int rand;

            switch (arr[i])
            {
                case 0:
                    winRate = 40;

                    rand = Random.Range(1, 4);
                    dividend = 1 + (float)rand / 10;
                    break;
                case 1:
                    winRate = 30;

                    rand = Random.Range(4, 8);
                    dividend = 1 + (float)rand / 10;
                    break;
                case 2:
                    winRate = 20;
                    
                    rand = Random.Range(7, 12);
                    dividend = 1 + (float)rand / 10;
                    break;
                case 3:
                    winRate = 10;
                    
                    rand = Random.Range(12, 16);
                    dividend = 1 + (float)rand / 10;
                    break;
            }

            _horseWinRate[i] = winRate;
            _horseDividend[i] = dividend;

            winRateTexts[i].GetComponent<TextMeshProUGUI>().text = winRate + "% / " + dividend;
        }
    }
    
    private T[] ShuffleArray<T>(T[] array)
    {
        for (var i = 0; i < array.Length; ++i)
        {
            var random1 = Random.Range(0, array.Length);
            var random2 = Random.Range(0, array.Length);

            (array[random1], array[random2]) = (array[random2], array[random1]);
        }

        return array;
    }

    public void UpChipCount()
    {
        if (_chipCount + 1 <= GameManager.Instance.Chip)
        {
            _chipCount += 1;

            _chipText.text = _chipCount.ToString();
        }
    }

    public void DownChipCount()
    {
        if (_chipCount - 1 >= 10)
        {
            _chipCount -= 1;
            
            _chipText.text = _chipCount.ToString();
        }
    }

    private void ChangeButtonAlpha()
    {
        for (var i = 0; i < HorseNumber; i++)
        {
            if (i == _selectHorseIdx)
            {
                _buttonImages[i].color = new Color(_buttonImages[i].color.r, _buttonImages[i].color.g, _buttonImages[i].color.b, 1f);
            }
            else
            {
                _buttonImages[i].color = new Color(_buttonImages[i].color.r, _buttonImages[i].color.g, _buttonImages[i].color.b, ChangeAlphaValue);
            }
        }
    }

    public void Select1Horse()
    {
        _selectHorseIdx = 0;

        ChangeButtonAlpha();
    }
    
    public void Select2Horse()
    {
        _selectHorseIdx = 1;
        
        ChangeButtonAlpha();
    }
    
    public void Select3Horse()
    {
        _selectHorseIdx = 2;
        
        ChangeButtonAlpha();
    }
    
    public void Select4Horse()
    {
        _selectHorseIdx = 3;
        
        ChangeButtonAlpha();
    }

    public void RaceStart()
    {
        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
        
        //경주 시작
        var rand = Random.Range(1, 11);
        var win = 0;
        switch (rand)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                //40% 승리
                win = 4;
                break;
            case 5:
            case 6:
            case 7:
                //30% 승리
                win = 3;
                break;
            case 8:
            case 9:
                //20% 승리
                win = 2;
                break;
            case 10:
                //10% 승리
                win = 1;
                break;
        }

        for (var i = 0; i < HorseNumber; i++)
        {
            
        }
    }
}
