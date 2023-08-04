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
    [SerializeField] private GameObject stressSlider;

    [SerializeField] private GameObject eventPanel;

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI chipText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private TextMeshProUGUI taxText;

    [SerializeField] private Transform buffView;

    [SerializeField] private TextMeshProUGUI eventScriptText;

    private Slider _slider;
    private TextMeshProUGUI _stressText;

    private void Start()
    {
        if (stressSlider)
        {
            _slider = stressSlider.GetComponent<Slider>();
            _stressText = stressSlider.GetComponentInChildren<TextMeshProUGUI>();
        }

        foreach (var buff in GameManager.Instance.BuffIconList)
        {
            if (buff.Value == null)
            {
                var obj = ObjectPool.Instance.GetPooledObject("BuffIcon");

                GameManager.Instance.BuffIconList[buff.Key] = obj;

                obj.transform.parent = buffView;
                obj.transform.localScale = Vector3.one;

                obj.GetComponent<BuffIcon>().id = buff.Key;
            }
        }
    }

    private void Update()
    {
        if (goldText)
        {
            var gold = GameManager.Instance.Gold;
            string goldTextString;
            if (gold > 1000000)
            {
                gold /= 1000000;
                goldTextString = gold + "M";
            }
            else if (gold > 1000)
            {
                gold /= 1000;
                goldTextString = gold + "K";
            }
            else
            {
                goldTextString = gold.ToString();
            }

            goldTextString += "골드";

            goldText.text = goldTextString;
        }

        if (chipText)
        {
            var chip = GameManager.Instance.Chip;
            string chipTextString;
            if (chip > 1000000)
            {
                chip /= 1000000;
                chipTextString = chip + "M";
            }
            else if (chip > 1000)
            {
                chip /= 1000;
                chipTextString = chip + "K";
            }
            else
            {
                chipTextString = chip.ToString();
            }

            chipTextString += "칩";

            chipText.text = chipTextString;
        }

        if (powerText)
        {
            var power = GameManager.Instance.Power;
            string powerTextString;
            if (power > 1000000)
            {
                power /= 1000000;
                powerTextString = power + "M";
            }
            else if (power > 1000)
            {
                power /= 1000;
                powerTextString = power + "K";
            }
            else
            {
                powerTextString = power.ToString();
            }

            powerTextString += "파워";

            powerText.text = powerTextString;
        }

        if (levelText)
        {
            var level = GameManager.Instance.Level;
            string levelTextString;
            if (level > 1000000)
            {
                level /= 1000000;
                levelTextString = level + "M";
            }
            else if (level > 1000)
            {
                level /= 1000;
                levelTextString = level + "K";
            }
            else
            {
                levelTextString = level.ToString();
            }

            levelTextString += "레벨";

            levelText.text = levelTextString;
        }

        if (taxText)
        {
            var tax = (int)(GameManager.Instance.buyChipCount * 0.3 + GameManager.Instance.Gold * 0.1);
            if (GameManager.Instance.AppliedEvent[3].IsActive)
            {
                tax = (int)(tax * GameManager.Instance.EventList[3].Buff_Amount);
            }

            taxText.text = tax.ToString();
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
        ChangeDay();
        
        //일요일 이벤트
        if (GameManager.Instance.Day == EDay.Sunday)
        {
            //세금
            var tax = (int)(GameManager.Instance.buyChipCount * 0.3 + GameManager.Instance.Gold * 0.1);
            GameManager.Instance.buyChipCount = 0;

            //증세
            if (GameManager.Instance.AppliedEvent[3].IsActive)
            {
                tax = (int)(tax * GameManager.Instance.EventList[3].Buff_Amount);
                GameManager.Instance.AppliedEvent[3].IsActive = false;
                ObjectPool.Instance.ReleaseObjectToPool(GameManager.Instance.BuffIconList[3]);
                GameManager.Instance.BuffIconList.Remove(3);
            }

            if (GameManager.Instance.Gold > tax)
            {
                GameManager.Instance.Gold -= tax;
            }
            else
            {
                //게임오버
                GameManager.Instance.GameOver();
                return;
            }
        }
        
        //토요일 이벤트
        if (GameManager.Instance.Day == EDay.Saturday)
        {
            //복권
            var rand = Random.Range(0, 1000000);

            foreach (var lottery in GameManager.Instance.lotteryList)
            {
                if (lottery == rand)
                {
                    var reward = (int)(GameManager.Instance.GamblingList[4].Reward);
                
                    //선조의 축복
                    reward = (int)(reward * (1 + GameManager.Instance.blessChipLevel * GameManager.Instance.BlessList[1].Amount));
                
                    //돼지머리
                    if (GameManager.Instance.hasPighead)
                        reward = (int)(reward * (1 + GameManager.Instance.ShopList[5].Reward));

                    GameManager.Instance.Chip += reward;
                }
            }
            
            GameManager.Instance.lotteryList.Clear();
        }

        //던전초기화
        GameManager.Instance.canEnterDungeon = true;
        
        //도박초기화
        GameManager.Instance.gamblingCurrentCount = GameManager.Instance.gamblingMaxCount;
        
        //음식초기화
        GameManager.Instance.hasPighead = false;
        if (GameManager.Instance.hasSteak)
            GameManager.Instance.PowerMultipleList.Remove("Steak");
        GameManager.Instance.hasSteak = false;

        //이벤트
        var randomValue = Random.Range(0f, 1f);
        
        if (randomValue <= 0.1f) //10%
        {
            DayEvent();
        }

        foreach (var getEvent in GameManager.Instance.AppliedEvent)
        {
            if (getEvent.Value.IsActive && getEvent.Key != 3)
            {
                if (getEvent.Value.Duration == 0)
                {
                    //버프 아이콘 제거

                    if (GameManager.Instance.BuffIconList.ContainsKey(getEvent.Key))
                    {
                        ObjectPool.Instance.ReleaseObjectToPool(GameManager.Instance.BuffIconList[getEvent.Key]);
                        GameManager.Instance.BuffIconList.Remove(getEvent.Key);
                    }

                    if (getEvent.Key == 1)
                    {
                        //적용중인 몸살버프 제거
                        GameManager.Instance.PowerMultipleList.Remove(GameManager.Instance.EventList[1].English);
                    }
                    if (getEvent.Key == 2)
                    {
                        //적용중인 열정버프 제거
                        GameManager.Instance.PowerMultipleList.Remove(GameManager.Instance.EventList[2].English);
                    }

                    getEvent.Value.IsActive = false;
                }
                else
                {
                    getEvent.Value.Duration -= 1;
                }
            }
        }
    }

    private void ChangeDay()
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
    }

    private void DayEvent()
    {
        var randomValue = 0;
        while (randomValue == 0)
        {
            randomValue = Random.Range(1, GameManager.Instance.EventList.Count + 1);

            if (GameManager.Instance.AppliedEvent[randomValue].IsActive || 
                (randomValue == 1 && GameManager.Instance.AppliedEvent[2].IsActive) || 
                (randomValue == 2 && GameManager.Instance.AppliedEvent[1].IsActive))
            {
                randomValue = 0;
            }
        }

        if (randomValue == 4)
        {
            //폐쇄
            GameManager.Instance.canEnterDungeon = false;
        }
        else if (randomValue == 6)
        {
            //단속
            GameManager.Instance.gamblingCurrentCount = 0;
        }
        else if (randomValue == 7)
        {
            //벌금
            GameManager.Instance.Gold = (int)(GameManager.Instance.Gold * 0.75f);
        }
        else if (randomValue == 10)
        {
            GameManager.Instance.Stress -= 15;
        }
        else if (randomValue == 11)
        {
            GameManager.Instance.Stress += 15;
        }
        else
        {
            GameManager.Instance.AppliedEvent[randomValue].IsActive = true;
            GameManager.Instance.AppliedEvent[randomValue].Duration =
                GameManager.Instance.EventList[randomValue].Buff_Duration;
            
            //버프아이콘 생성
            var obj = ObjectPool.Instance.GetPooledObject("BuffIcon");
            
            GameManager.Instance.BuffIconList.Add(randomValue, obj);

            obj.transform.parent = buffView;
            obj.transform.localScale = Vector3.one;

            obj.GetComponent<BuffIcon>().id = randomValue;
        }
        OpenEventPanel(randomValue);
        //몸살 배열 추가
        if (randomValue == 1)
        {
            GameManager.Instance.PowerMultipleList.Add(GameManager.Instance.EventList[1].English,GameManager.Instance.EventList[1].Buff_Amount);
        }
        //열정 배열 추가
        else if (randomValue == 2)
        {
            GameManager.Instance.PowerMultipleList.Add(GameManager.Instance.EventList[2].English,GameManager.Instance.EventList[2].Buff_Amount);
        }
        
        ManageSave.Instance.Saved();
    }

    private void OpenEventPanel(int id)
    {
        eventPanel.SetActive(true);

        switch (id)
        {
            case 1:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Sick"];
                break;
            case 2:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Passion"];
                break;
            case 3:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Inc_Tax"];
                break;
            case 4:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Closure"];
                break;
            case 5:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Luck"];
                break;
            case 6:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Crackdown"];
                break;
            case 7:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Fine"];
                break;
            case 8:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Depression"];
                break;
            case 9:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Enteritis"];
                break;
            case 10:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Joy"];
                break;
            case 11:
                eventScriptText.text = GameManager.Instance.ScriptList["Script_Misfotrune"];
                break;
            default:
                eventScriptText.text = "Buff ID Error!!!";
                break;
        }

        if (id is 2 or 5 or 10)
        {
            //Positive
            EffectManager.Instance.PlayEventPositive();
        }
        else
        {
            //Negative
            EffectManager.Instance.PlayEventNegative();
        }
    }
}
