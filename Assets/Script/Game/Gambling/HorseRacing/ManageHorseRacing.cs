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
    private const int HorseNumber = 4;
    private const float ChangeAlphaValue = 0.3f;
    
    [SerializeField] private MoveHorse[] horses;
    [SerializeField] private TextMeshProUGUI[] winRateTexts;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image[] buttonImages;
    [SerializeField] private TextMeshProUGUI chipText;
    [SerializeField] private GameObject resultImage;
    [SerializeField] private TextMeshProUGUI resultText;
    private Button[] _buttons;

    private int[] _horseWinRate;
    private float[] _horseDividend;

    private int _chipCount;

    private int _selectHorseIdx;

    private bool _isRestart;
    private bool _isWin;
    
    private void Awake()
    {
        _horseWinRate = new int[HorseNumber];
        _horseDividend = new float[HorseNumber];

        _buttons = GetComponentsInChildren<Button>();
    }

    private void OnEnable()
    {
        _chipCount = 10;
        chipText.text = _chipCount.ToString();

        _selectHorseIdx = -1;

        _isRestart = false;
        _isWin = false;
        
        resultImage.SetActive(false);

        foreach (var buttonImage in buttonImages)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
        }

        foreach (var button in _buttons)
        {
            button.interactable = true;
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

            winRateTexts[i].text = "승률 : " + winRate + "%<br>배당률 : " + dividend + "배";
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

            chipText.text = _chipCount.ToString();
        }
    }

    public void DownChipCount()
    {
        if (_chipCount - 1 >= 10)
        {
            _chipCount -= 1;
            
            chipText.text = _chipCount.ToString();
        }
    }

    private void ChangeButtonAlpha()
    {
        for (var i = 0; i < HorseNumber; i++)
        {
            if (i == _selectHorseIdx)
            {
                buttonImages[i].color = new Color(buttonImages[i].color.r, buttonImages[i].color.g, buttonImages[i].color.b, 1f);
            }
            else
            {
                buttonImages[i].color = new Color(buttonImages[i].color.r, buttonImages[i].color.g, buttonImages[i].color.b, ChangeAlphaValue);
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
        if (_selectHorseIdx == -1 || _chipCount > GameManager.Instance.Chip)
            return;

        GameManager.Instance.Chip -= _chipCount;
        GameManager.Instance.Stress += GameManager.Instance.GamblingList[2].Stress;
        
        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
        
        //경주 시작
        var rand = Random.Range(0f, 1f);    //우승말 고르기
        Debug.Log(rand);

        if (rand <= 0.4f)
        {
            CheckWinHorse(40);
        }
        else if (rand <= 0.7f)
        {
            CheckWinHorse(30);
        }
        else if (rand <= 0.9f)
        {
            CheckWinHorse(20);
        }
        else
        {
            CheckWinHorse(10);
        }
    }

    private void CheckWinHorse(int winRate)
    {
        for (var i = 0; i < HorseNumber; i++)
        {
            if (_horseWinRate[i] == winRate)
            {
                horses[i].Move(true);
                
                Debug.Log(i);
            }
            else
            {
                horses[i].Move(false);
            }
        }
    }

    public void FinishMove()
    {
        exitButton.interactable = true;
        _isRestart = true;
        //패배 승리 이미지
        if (_isWin)
        {
            resultText.text = "승리";
            GameManager.Instance.Chip += (int)(_chipCount * _horseDividend[_selectHorseIdx]);
        }
        else
        {
            resultText.text = "패배";
        }
        
        resultImage.SetActive(true);
    }

    public void Restart()
    {
        if(_isRestart)
            gameObject.SetActive(true);
    }
}
