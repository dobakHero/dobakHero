using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    
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
    private long _gold = 0;
    public long Gold
    {
        get => _gold;
        set
        {
            _gold = value;
        }
    }

    private int _chip = 0;
    public int Chip
    {
        get => _chip;
        set
        {
            _chip = value;
        }
    }
    
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

    private int _bless = 0;

    public int Bless
    {
        get => _bless;
        set
        {
            _bless = value;
        }
    }

    [HideInInspector] public int blessStressLevel = 0;
    [HideInInspector] public int blessChipLevel = 0;
    [HideInInspector] public int blessGoldLevel = 0;
    [HideInInspector] public int blessDungeonLevel = 0;

    public Dictionary<int, Bless> BlessList;

    private int _stress;
    public int Stress
    {
        get => _stress;
        set
        {
            var increaseStress = value - _stress;
            
            //우울증 이벤트
            if (AppliedEvent[8].IsActive)
            {
                if (increaseStress > 0)
                {
                    value = (int)(increaseStress * (1 + EventList[8].Buff_Amount)) + _stress;
                }
            }

            //선조의 축복
            increaseStress = value - _stress;

            if (increaseStress > 0)
            {
                value = (int)(increaseStress * (1 - BlessList[4].Amount * blessStressLevel)) + _stress;
            }
            
            //최솟값
            if (value < 0)
            {
                value = 0;
            }
            
            //최댓값
            if (value >= 100)
            {
                value = 100;
                //게임오버
                GameOver();
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
        }
    }

    //CSV에서 읽어온 Event 저장
    public Dictionary<int, Event> EventList;    //(ID, Event)
    public Dictionary<int, Buff> AppliedEvent;   //(ID, Duration)

    public Dictionary<int, GameObject> BuffIconList;

    #endregion

    #region Gambling

    [HideInInspector] public int gamblingMaxCount = 3;
    [HideInInspector] public int gamblingCurrentCount = 3;

    [HideInInspector] public Dictionary<int, Gambling> GamblingList;

    [HideInInspector] public List<int> lotteryList;

    [HideInInspector] public int buyChipCount = 0;

    #endregion

    #region Dungeon

    public Dictionary<int, Dungeon> DungeonList;

    [HideInInspector] public bool canEnterDungeon = true;

    #endregion

    #region Shop

    public Dictionary<int, Shop> ShopList;
    [HideInInspector] public int figureStock = 2;
    [HideInInspector] public bool hasPighead = false;
    [HideInInspector] public bool hasSteak = false;
    
    #endregion

    private void DataInitialize()
    {
        //AppliedEvent Assignment
        AppliedEvent = new Dictionary<int, Buff>();

        PowerPlusList = new Dictionary<string, int>();
        PowerMultipleList = new Dictionary<string, float>();

        BuffIconList = new Dictionary<int, GameObject>();

        lotteryList = new List<int>();
        
    }

    private void LoadData()
    {
        ScriptList = new Dictionary<string, string>();
        var scriptCsv = CSVReader.Read("CSV/Datatable_Script");
        foreach (var script in scriptCsv)
        {
            ScriptList.Add(script["English"].ToString(),script["Korean"].ToString());
        }

        BlessList = new Dictionary<int, Bless>();
        var blessCsv = CSVReader.Read("CSV/Datatable_Bless");
        foreach (var bless in blessCsv)
        {
            var temp = new Bless 
            {
                English = bless["English"].ToString(),
                Korean = bless["Korean"].ToString(),
                Amount = float.Parse(bless["Amount"].ToString()),
                Max = int.Parse(bless["Max"].ToString())
            };

            BlessList.Add(int.Parse(bless["ID"].ToString()), temp);
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

        foreach (var events in EventList)
        {
            AppliedEvent.Add(events.Key,new Buff(0,false));
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

        ShopList = new Dictionary<int, Shop>();
        var shopCsv = CSVReader.Read("CSV/Datatable_Shop");
        foreach (var shop in shopCsv)
        {
            var temp = new Shop
            {
                English = shop["English"].ToString(),
                Korean = shop["Korean"].ToString(),
                Price = long.Parse(shop["Price"].ToString(), NumberStyles.AllowExponent),
                Reward = float.Parse(shop["Reward"].ToString()),
                Stress = int.Parse(shop["Stress"].ToString()),
                Power = int.Parse(shop["Power"].ToString()),
                Stock = int.Parse(shop["Stock"].ToString()),
            };
            
            ShopList.Add(int.Parse(shop["ID"].ToString()), temp);
        }
    }

    public void GameOver()
    {
        if(gameOverPanel)
            gameOverPanel.SetActive(true);
        else
        {
            Instantiate(gameOverPanel);
            gameOverPanel.SetActive(true);
        }
    }

    public void ReStart()
    {
        Gold = 0;
        Chip = 0;
        _power = 5000;
        
        PowerPlusList.Clear();
        PowerMultipleList.Clear();

        _level = 1;
        _stress = 0;

        Day = EDay.Monday;
        foreach (var getEvent in AppliedEvent)
        {
            getEvent.Value.IsActive = false;
        }

        foreach (var buffIcon in BuffIconList)
        {
            ObjectPool.Instance.ReleaseObjectToPool(buffIcon.Value);
        }
        BuffIconList.Clear();

        gamblingMaxCount = 3;
        gamblingCurrentCount = 3;
        
        lotteryList.Clear();
        buyChipCount = 0;

        canEnterDungeon = true;

        figureStock = 2;
        hasPighead = false;
        hasSteak = false;
    }
}
