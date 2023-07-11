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

    public Dictionary<string, string> ScriptList;

    #region Gold & Stat

    //재화
    public int Gold { get; set; } = 0;
    public int Chip { get; set; } = 0;
    
    //스텟
    private int _power = 5000;
    public int Power
    {
        get
        {
            // + -> *(합연산) -> 스트레스 감소
            var res = _power;
            
            //더하기
            var powerPlusValue = 0;

            foreach (var item in PowerPlusList)
            {
                powerPlusValue += item.Value;
            }

            res += powerPlusValue;
            
            //곱하기

            var powerMultipleValue = 1f;

            foreach (var item in PowerMultipleList)
            {
                powerMultipleValue += item.Value;
            }

            res = (int)(res * powerMultipleValue);

            //스트레스로 인한 전투력 감소
            res = (int)((1 - Stress * 0.005) * res);

            return res;
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

    public Dictionary<string, int> PowerPlusList;
    public Dictionary<string, float> PowerMultipleList;

    private int _level = 1;
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
            foreach (var getEvent in AppliedEvent)
            {
                if (getEvent.Key == 8)
                {
                    var increaseStress = value - _stress;

                    if (increaseStress > 0)
                    {
                        value = (int)(increaseStress * 1.5) + _stress;
                    }

                    break;
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
        
    }

    //CSV에서 읽어온 Event 저장
    public Dictionary<int, Event> EventList;    //(ID, Event)
    public List<KeyValuePair<int, int>> AppliedEvent;   //(ID, Duration)

    #endregion

    #region Gambling

    private int _gamblingMaxCount = 3;
    private int _gamblingCurrentCount = 3;

    public Dictionary<int, Gambling> GamblingList;

    #endregion

    #region Dungeon

    public Dictionary<int, Dungeon> DungeonList;

    public bool canEnterDungeon = true;

    #endregion
    
    private void DataInitialize()
    {
        //AppliedEvent Assignment
        AppliedEvent = new List<KeyValuePair<int, int>>();

        PowerPlusList = new Dictionary<string, int>();
        PowerMultipleList = new Dictionary<string, float>();
    }

    private void LoadData()
    {
        ScriptList = new Dictionary<string, string>();
        var scriptCsv = CSVReader.Read("CSV/Datatable_Script");
        foreach (var script in scriptCsv)
        {
            ScriptList.Add(script["English"].ToString(),script["Korean"].ToString());
        }
        
        EventList = new Dictionary<int, Event>();
        var eventCsv = CSVReader.Read("CSV/Datatable_Event");
        foreach (var events in eventCsv)
        {
            var temp = new Event
            {
                English = events["English"].ToString(),
                Korean = events["Korean"].ToString(),
                Buff_Amount = float.Parse(events["Buff_Amount"].ToString()),
                Buff_Duration = int.Parse(events["Buff_Duration"].ToString())
            };

            EventList.Add(int.Parse(events["ID"].ToString()), temp);
        }
        
        GamblingList = new Dictionary<int, Gambling>();
        var gamblingCsv = CSVReader.Read("CSV/Datatable_Dobak");
        foreach (var gambling in gamblingCsv)
        {
            var temp = new Gambling
            {
                English = gambling["English"].ToString(),
                Korean = gambling["Korean"].ToString(),
                Cost = int.Parse(gambling["Cost"].ToString()),
                Reward = int.Parse(gambling["Reward"].ToString()),
                Stress = int.Parse(gambling["Stress"].ToString())
            };

            GamblingList.Add(int.Parse(gambling["ID"].ToString()), temp);
        }
        
        DungeonList = new Dictionary<int, Dungeon>();
        var dungeonCsv = CSVReader.Read("CSV/Datatable_Dungeon");
        foreach (var dungeon in dungeonCsv)
        {
            var temp = new Dungeon
            {
                English = dungeon["English"].ToString(),
                Korean = dungeon["Korean"].ToString(),
                Arrow = int.Parse(dungeon["Arrow"].ToString()),
                Reward = float.Parse(dungeon["Reward"].ToString())
            };

            DungeonList.Add(int.Parse(dungeon["ID"].ToString()), temp);
        }
    }
}
