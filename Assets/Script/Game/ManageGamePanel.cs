using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ManageGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject goldTextGameObject;
    [SerializeField] private GameObject chipTextGameObject;
    [SerializeField] private GameObject powerTextGameObject;
    [SerializeField] private GameObject levelTextGameObject;
    [SerializeField] private GameObject stressSlider;

    [SerializeField] private GameObject eventPanel;

    private TextMeshProUGUI _goldText;
    private TextMeshProUGUI _chipText;
    private TextMeshProUGUI _powerText;
    private TextMeshProUGUI _levelText;

    private Slider _slider;
    private TextMeshProUGUI _stressText;

    private void Start()
    {
        if (goldTextGameObject)
        {
            _goldText = goldTextGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        if (chipTextGameObject)
        {
            _chipText = chipTextGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        if (powerTextGameObject)
        {
            _powerText = powerTextGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        if (levelTextGameObject)
        {
            _levelText = levelTextGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        if (stressSlider)
        {
            _slider = stressSlider.GetComponent<Slider>();
            _stressText = stressSlider.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (_goldText)
        {
            var gold = GameManager.Instance.Gold;
            string goldText;
            if (gold > 1000000)
            {
                gold /= 1000000;
                goldText = gold + "M";
            }
            else if (gold > 1000)
            {
                gold /= 1000;
                goldText = gold + "K";
            }
            else
            {
                goldText = gold.ToString();
            }

            _goldText.text = goldText;
        }

        if (_chipText)
        {
            var chip = GameManager.Instance.Chip;
            string chipText;
            if (chip > 1000000)
            {
                chip /= 1000000;
                chipText = chip + "M";
            }
            else if (chip > 1000)
            {
                chip /= 1000;
                chipText = chip + "K";
            }
            else
            {
                chipText = chip.ToString();
            }

            _chipText.text = chipText;
        }

        if (_powerText)
        {
            var power = GameManager.Instance.Power;
            string powerText;
            if (power > 1000000)
            {
                power /= 1000000;
                powerText = power + "M";
            }
            else if (power > 1000)
            {
                power /= 1000;
                powerText = power + "K";
            }
            else
            {
                powerText = power.ToString();
            }

            _powerText.text = powerText;
        }

        if (_levelText)
        {
            var level = GameManager.Instance.Level;
            string levelText;
            if (level > 1000000)
            {
                level /= 1000000;
                levelText = level + "M";
            }
            else if (level > 1000)
            {
                level /= 1000;
                levelText = level + "K";
            }
            else
            {
                levelText = level.ToString();
            }

            _levelText.text = levelText;
        }

        if (_slider)
        {
            _slider.value = GameManager.Instance.Stress;
        }

        if (_stressText)
        {
            _stressText.text = GameManager.Instance.Stress + " / 100";
        }
    }

    public void NextDay()
    {
        //요일 변경
        if (GameManager.Instance.Day != EDay.Sunday)
        {
            GameManager.Instance.Day += 1;
        }
        else
        {
            GameManager.Instance.Day = EDay.Monday;
        }
        Debug.Log("현재 요일 : " + GameManager.Instance.Day);
        
        //일요일 이벤트
        if (GameManager.Instance.Day == EDay.Sunday)
        {
            //세금
            var tempChip = 1;

            var tax = (int)(tempChip * 0.3 + GameManager.Instance.Gold * 0.1);

            //증세
            foreach (var getEvent in GameManager.Instance.AppliedEvent)
            {
                if (getEvent.Key == 3)
                {
                    tax = (int)(tax * GameManager.Instance.EventList[3].Buff_Amount);
                    GameManager.Instance.AppliedEvent.Remove(getEvent);
                    break;
                }
            }

            if (GameManager.Instance.Gold > tax)
            {
                GameManager.Instance.Gold -= tax;
            }
            else
            {
                //게임오버
            }
        }
        
        //토요일 이벤트
        if (GameManager.Instance.Day == EDay.Saturday)
        {
            //복권
        }

        //던전초기화
        GameManager.Instance.canEnterDungeon = true;
        
        //도박초기화
        GameManager.Instance.gamblingCurrentCount = GameManager.Instance.gamblingMaxCount;
            
        //이벤트
        var randomValue = Random.Range(0f, 1f);
        
        if (randomValue <= 0.1f) //10%
        {
            DayEvent();
        }

        for (var i = 0; i < GameManager.Instance.AppliedEvent.Count; i++)
        {
            if (GameManager.Instance.AppliedEvent[i].Value == 0)
            {
                //버프 아이콘 제거

                if (GameManager.Instance.AppliedEvent[i].Key == 1)
                {
                    GameManager.Instance.PowerMultipleList.Remove(GameManager.Instance.EventList[1].English);
                }

                if (GameManager.Instance.AppliedEvent[i].Key == 2)
                {
                    GameManager.Instance.PowerMultipleList.Remove(GameManager.Instance.EventList[2].English);
                }
                
                GameManager.Instance.AppliedEvent.Remove(GameManager.Instance.AppliedEvent[i]);
            }
            else
            {
                GameManager.Instance.AppliedEvent[i] = new KeyValuePair<int, int>(GameManager.Instance.AppliedEvent[i].Key,GameManager.Instance.AppliedEvent[i].Value - 1);

                switch (GameManager.Instance.AppliedEvent[i].Key)
                {
                    case 1: //몸살
                        //GameManager.cs 45줄
                        Debug.Log("몸살 진행중");
                        break;
                    case 2: //열정
                        //GameManager.cs 51줄
                        Debug.Log("열정 진행중");
                        break;
                    case 3: //증세
                        //GameManager.cs 153줄
                        Debug.Log("증세 진행중");
                        break;
                    case 4: //폐쇄
                        
                        break;
                    case 5: //행운
                        
                        break;
                    case 6: //단속
                        
                        break;
                    case 7: //벌금
                        GameManager.Instance.Gold -= (int)(GameManager.Instance.Gold * GameManager.Instance.EventList[7].Buff_Amount);
                        Debug.Log("벌금 이벤트 발생");
                        GameManager.Instance.AppliedEvent.Remove(GameManager.Instance.AppliedEvent[i]);
                        break;
                    case 8: //우울증
                        //GameManager.cs 116줄
                        break;
                    case 9: //장염
                        break;
                    case 10://기쁨
                        GameManager.Instance.Stress -= 15;
                        Debug.Log("기쁨 이벤트 발생");
                        GameManager.Instance.AppliedEvent.Remove(GameManager.Instance.AppliedEvent[i]);
                        break;
                    case 11://불행
                        GameManager.Instance.Stress += 15;
                        Debug.Log("불행 이벤트 발생");
                        GameManager.Instance.AppliedEvent.Remove(GameManager.Instance.AppliedEvent[i]);
                        break;
                    default:
                        Debug.Log("Item CODE ERROR!!! Item.Key : " + GameManager.Instance.AppliedEvent[i].Key);
                        break;
                }
            }
        }
    }

    private void DayEvent()
    {
        var randomValue = 0;
        while (randomValue == 0)
        {
            randomValue = Random.Range(0, GameManager.Instance.EventList.Count);

            var hasSick = false;
            var hasPassion = false;

            foreach (var getEvent in GameManager.Instance.AppliedEvent)
            {
                if (getEvent.Key == randomValue)
                {
                    randomValue = 0;
                }

                if (getEvent.Key == 1)
                {
                    hasSick = true;
                }

                if (getEvent.Key == 2)
                {
                    hasPassion = true;
                }
            }
            
            //몸살과 열정은 중복 불가능
            if ((randomValue == 1 && hasPassion) || (randomValue == 2 && hasSick))
            {
                randomValue = 0;
            }
        }
        
        GameManager.Instance.AppliedEvent.Add(new KeyValuePair<int, int>(randomValue, GameManager.Instance.EventList[randomValue].Buff_Duration));
        
        OpenEventPanel(randomValue);

        //몸살 배열 추가
        if (randomValue == 1)
        {
            GameManager.Instance.PowerMultipleList.Add(GameManager.Instance.EventList[1].English,GameManager.Instance.EventList[1].Buff_Amount);
        }
        
        //열정 배열 추가
        if (randomValue == 2)
        {
            GameManager.Instance.PowerMultipleList.Add(GameManager.Instance.EventList[2].English,GameManager.Instance.EventList[2].Buff_Amount);
        }
    }

    private void OpenEventPanel(int id)
    {
        eventPanel.SetActive(true);
    }
}
