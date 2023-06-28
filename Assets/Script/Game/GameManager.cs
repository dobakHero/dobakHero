using System;
using System.Collections;
using System.Collections.Generic;
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
            var stressBuff = (int)(_power * 0.5) * Stress;

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

    private EDay _day = EDay.Monday;
    public EDay Day
    {
        get => _day;
        set
        {
            _day = value;
            
            if (_day == EDay.Sunday)
            {
                //세금
            }

            if (_day == EDay.Saturday)
            {
                //복권
            }
        }
    }

    public void NextDay()
    {
        if (_day != EDay.Sunday)
        {
            _day += 1;
        }
        else
        {
            _day = EDay.Monday;
        }
        
        //포만감 감소
            
        //이벤트
        var randomValue = Random.Range(0f, 1f);
        
        if (randomValue <= 0.1f) //10%
        {
            DayEvent();
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

    public void LoadData()
    {
        EventList = new Dictionary<int, Event>();
    }
}
