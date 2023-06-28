using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            DataInitialize();
            LoadData();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region Gold & Stat

    //재화
    public int Gold { get; set; } = 0;
    public int Chip { get; set; } = 0;
    
    //스텟
    private int _power;
    public int Power
    {
        get
        {
            var res = _power;
            
            //스트레스로 인한 전투력 감소
            var stressBuff = (int)(_power * 0.005) * Stress;
            
            //몸살로 인한 전투력 감소
            if (AppliedEvent.ContainsKey(1))
            {
                //전투력 계산 순서고려
            }
            
            //열정으로 인한 전투력 증가
            if (AppliedEvent.ContainsKey(2))
            {
                
            }

            return res - stressBuff;
        }
        set
        {
            //최솟값
            if (value < 5000)
            {
                value = 5000;
            }

            //최댓값
            if (value > 1000000)
            {
                value = 1000000;
            }
            
            _power = value;
        }
    }

    private int _level;
    public int Level
    {
        get => _level;
        set
        {
            //최솟값
            if (value < 1)
            {
                value = 1;
            }

            //최댓값
            if (value > 100)
            {
                value = 100;
            }

            //레벨 업
            var levelUpAmount = value - _level;
            if (levelUpAmount > 0)
            {
                //전투력 증가
                Power += levelUpAmount * 5000;
            }

            _level = value;
        }
    }

    public int Bless { get; set; } = 0;

    private int _stress;

    public int Stress
    {
        get => _stress;
        set
        {
            //우울증
            if (AppliedEvent.ContainsKey(8))
            {
                var increaseStress = value - _stress;

                if (increaseStress > 0)
                {
                    value = (int)(increaseStress * 1.5) + _stress;
                }
            }
            
            //최댓값
            if (value > 100)
            {
                value = 100;
                //게임오버
            }

            _stress = value;
        }
    }

    #endregion

    #region Event

    public EDay Day { get; set; } = EDay.Monday;

    public void NextDay()
    {
        //요일 변경
        if (Day != EDay.Sunday)
        {
            Day += 1;
        }
        else
        {
            Day = EDay.Monday;
        }
        
        //일요일 이벤트
        if (Day == EDay.Sunday)
        {
            //세금
            var tempChip = 1;

            var tax = (int)(tempChip * 0.3 + Gold * 0.1);

            //증세
            if (AppliedEvent.ContainsKey(3))
            {
                tax = (int)(tax * EventList[3].BuffAmount);
                AppliedEvent.Remove(3);
            }

            Gold -= tax;
        }
        
        //토요일 이벤트
        if (Day == EDay.Saturday)
        {
            //복권
        }
        
        //포만감 감소
            
        //이벤트
        var randomValue = Random.Range(0f, 1f);
        
        if (randomValue <= 0.1f) //10%
        {
            DayEvent();
        }

        foreach (var item in AppliedEvent)
        {
            if (item.Value == 0)
            {
                //버프 아이콘 제거
                AppliedEvent.Remove(item.Key);
            }
            else
            {
                AppliedEvent[item.Key] = item.Value - 1;

                switch (item.Key)
                {
                    case 1: //몸살
                        //GameManager.cs 45줄
                        break;
                    case 2: //열정
                        //GameManager.cs 51줄
                        break;
                    case 3: //증세
                        //GameManager.cs 153줄
                        break;
                    case 4: //폐쇄
                        
                        break;
                    case 5: //행운
                        
                        break;
                    case 6: //단속
                        
                        break;
                    case 7: //벌금
                        Gold -= (int)(Gold * EventList[7].BuffAmount);
                        AppliedEvent.Remove(7);
                        break;
                    case 8: //우울증
                        //GameManager.cs 116줄
                        break;
                    case 9: //장염
                        break;
                    case 10://기쁨
                        Stress -= 15;
                        AppliedEvent.Remove(10);
                        break;
                    case 11://불행
                        Stress += 15;
                        AppliedEvent.Remove(11);
                        break;
                    default:
                        Debug.Log("Item CODE ERROR!!! Item.Key : " + item.Key);
                        break;
                }
            }
        }
    }

    //CSV에서 읽어온 Event 저장
    public Dictionary<int, Event> EventList;    //(ID, Event)
    public Dictionary<int, int> AppliedEvent;   //(ID, Duration)
    
    private void DayEvent()
    {
        int randomValue;
        do
        {
            randomValue = Random.Range(0, EventList.Count);
        } while (AppliedEvent.ContainsKey(randomValue));
        
        AppliedEvent.Add(randomValue, EventList[randomValue].Duration);
    }

    #endregion

    #region Gambling

    private int _gamblingMaxCount = 3;
    private int _gamblingCurrentCount = 3;

    #endregion

    private void DataInitialize()
    {
        //AppliedEvent Assignment
        AppliedEvent = new Dictionary<int, int>();
    }

    public void LoadData()
    {
        EventList = new Dictionary<int, Event>();
    }
}
