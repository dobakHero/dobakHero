using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public long Gold = 0;
    public int Chip = 0;
    public int Power = 5000;

    public List<string> PowerPlusListKey = new List<string>();
    public List<int> PowerPlusListValue = new List<int>();
    public List<string> PowerMultipleListKey = new List<string>();
    public List<float> PowerMultipleListValue = new List<float>();

    public int Level = 1;
    
    public int Bless = 0;
    public int BlessStressLevel = 0;
    public int BlessChipLevel = 0;
    public int BlessGoldLevel = 0;
    public int BlessDungeonLevel = 0;
    
    public int Stress = 0;

    public EDay Day = EDay.Monday;

    public List<bool> AppliedEventIsActive = new List<bool>();
    public List<int> AppliedEventRemainDuration = new List<int>();

    public List<int> BuffIconList = new List<int>();

    public int GamblingMaxCount = 3;
    public int GamblingCurrentCount = 3;

    public List<int> LotteryList = new List<int>();

    public int BuyChipCount = 0;

    public bool CanEnterDungeon = true;

    public int FigureStock = 2;
    public bool HasPigHead = false;
    public bool HasSteak = false;
}

public class ManageSave : MonoBehaviour
{
    public static ManageSave Instance;

    public SaveData Save = new SaveData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        LoadJson();
    }
    
    public void LoadJson()
    {
        if (!File.Exists(Application.dataPath + "/Save.json"))
        {
            File.WriteAllText(Application.dataPath + "/Save.json", JsonUtility.ToJson(Save));
        }
        else
        {
            string str = File.ReadAllText(Application.dataPath + "/Save.json");

            Save = JsonUtility.FromJson<SaveData>(str);

            GameManager.Instance.Gold = Save.Gold;
            GameManager.Instance.Chip = Save.Chip;
            GameManager.Instance.Power = Save.Power;

            GameManager.Instance.PowerPlusList.Clear();
            for (var i = 0; i < Save.PowerPlusListKey.Count; i++)
            {
                GameManager.Instance.PowerPlusList.Add(Save.PowerPlusListKey[i],Save.PowerPlusListValue[i]);
            }

            GameManager.Instance.PowerMultipleList.Clear();
            for (var i = 0; i < Save.PowerMultipleListKey.Count; i++)
            {
                GameManager.Instance.PowerMultipleList.Add(Save.PowerMultipleListKey[i],Save.PowerMultipleListValue[i]);
            }

            GameManager.Instance.Level = Save.Level;

            GameManager.Instance.Bless = Save.Bless;
            GameManager.Instance.blessStressLevel = Save.BlessStressLevel;
            GameManager.Instance.blessChipLevel = Save.BlessChipLevel;
            GameManager.Instance.blessGoldLevel = Save.BlessGoldLevel;
            GameManager.Instance.blessDungeonLevel = Save.BlessDungeonLevel;

            GameManager.Instance.Stress = Save.Stress;

            GameManager.Instance.Day = Save.Day;

            for (var i = 0; i < GameManager.Instance.EventList.Count; i++)
            {
                var buff = new Buff(0,false);
                if (Save.AppliedEventIsActive[i])
                {
                    buff.IsActive = true;
                    buff.Duration = Save.AppliedEventRemainDuration[i];
                }

                GameManager.Instance.AppliedEvent[i + 1] = buff;
            }

            GameManager.Instance.BuffIconList.Clear();
            for (var i = 0; i < Save.BuffIconList.Count; i++)
            {
                GameManager.Instance.BuffIconList.Add(Save.BuffIconList[i],null);
            }

            GameManager.Instance.gamblingMaxCount = Save.GamblingMaxCount;
            GameManager.Instance.gamblingCurrentCount = Save.GamblingCurrentCount;

            GameManager.Instance.lotteryList = Save.LotteryList;

            GameManager.Instance.buyChipCount = Save.BuyChipCount;

            GameManager.Instance.canEnterDungeon = Save.CanEnterDungeon;

            GameManager.Instance.figureStock = Save.FigureStock;
            GameManager.Instance.hasPighead = Save.HasPigHead;
            GameManager.Instance.hasSteak = Save.HasSteak;
        }
    }

    public void Saved()
    {
        Save.Gold = GameManager.Instance.Gold;
        Save.Chip = GameManager.Instance.Chip;
        Save.Power = GameManager.Instance.Power;

        Save.PowerPlusListKey.Clear();
        Save.PowerPlusListValue.Clear();
        foreach (var list in GameManager.Instance.PowerPlusList)
        {
            Save.PowerPlusListKey.Add(list.Key);
            Save.PowerPlusListValue.Add(list.Value);
        }

        Save.PowerMultipleListKey.Clear();
        Save.PowerMultipleListValue.Clear();
        foreach (var list in GameManager.Instance.PowerMultipleList)
        {
            Save.PowerMultipleListKey.Add(list.Key);
            Save.PowerMultipleListValue.Add(list.Value);
        }

        Save.Level = GameManager.Instance.Level;

        Save.Bless = GameManager.Instance.Bless;
        Save.BlessStressLevel = GameManager.Instance.blessStressLevel;
        Save.BlessChipLevel = GameManager.Instance.blessChipLevel;
        Save.BlessGoldLevel = GameManager.Instance.blessGoldLevel;
        Save.BlessDungeonLevel = GameManager.Instance.blessDungeonLevel;

        Save.Stress = GameManager.Instance.Stress;

        Save.Day = GameManager.Instance.Day;

        Save.AppliedEventIsActive.Clear();
        Save.AppliedEventRemainDuration.Clear();
        for (int i = 1; i <= GameManager.Instance.AppliedEvent.Count; i++)
        {
            Save.AppliedEventIsActive.Add(GameManager.Instance.AppliedEvent[i].IsActive);
            Save.AppliedEventRemainDuration.Add(GameManager.Instance.AppliedEvent[i].Duration);
        }

        Save.BuffIconList.Clear();
        foreach (var list in GameManager.Instance.BuffIconList)
        {
            Save.BuffIconList.Add(list.Key);
        }

        Save.GamblingMaxCount = GameManager.Instance.gamblingMaxCount;
        Save.GamblingCurrentCount = GameManager.Instance.gamblingCurrentCount;

        Save.LotteryList = GameManager.Instance.lotteryList;

        Save.BuyChipCount = GameManager.Instance.buyChipCount;

        Save.CanEnterDungeon = GameManager.Instance.canEnterDungeon;

        Save.FigureStock = GameManager.Instance.figureStock;

        Save.HasPigHead = GameManager.Instance.hasPighead;
        Save.HasSteak = GameManager.Instance.hasSteak;
        
        WriteJson();
    }
    
    private void WriteJson()
    {
        File.WriteAllText(Application.dataPath + "/Save.json", JsonUtility.ToJson(Save));
    }
}

