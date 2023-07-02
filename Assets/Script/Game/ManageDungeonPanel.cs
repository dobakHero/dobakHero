using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ManageDungeonPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image monsterImage;
    [SerializeField] private List<Sprite> monsterSpriteList;
    [SerializeField] private GameObject patternBox;
    [SerializeField] private GameObject playerInputBox;
    [SerializeField] private List<Sprite> arrowSpriteList;
    [SerializeField] private Slider monsterTimer;

    private ClosePanel _closePanel;

    private int _dungeonLevel;
    private float _dungeonTime;
    private float _monsterTime;
    private bool _isDungeonTimerOn;
    private bool _isMonsterTimerOn;

    private int _enterPower;

    private const int DefaultTime = 60;
    private const int DefaultMonsterTime = 5;

    private List<int> _pattern = new List<int>();

    private enum EArrow
    {
        Up = 1,
        Right = 2,
        Left = 3,
        Down = 4,
    }

    private void Awake()
    {
        _closePanel = GetComponent<ClosePanel>();
    }

    public void EnterDungeon()
    {
        _dungeonLevel = 1;
        
        _dungeonTime = DefaultTime;
        _monsterTime = DefaultMonsterTime;

        timerText.text = (int)_dungeonTime / 60 + " : " + $"{(int)_dungeonTime % 60:D2}";
        monsterTimer.maxValue = DefaultMonsterTime;

        _enterPower = GameManager.Instance.Power;

        _isDungeonTimerOn = true;
        _isMonsterTimerOn = true;

        GameManager.Instance.canEnterDungeon = false;
        
        SpawnMonster();
    }

    private void SpawnMonster()
    {
        monsterImage.sprite = monsterSpriteList[(_dungeonLevel - 1) % monsterSpriteList.Count];

        if (_dungeonLevel % 10 == 0)
        {
            //보스몬스터
            SpawnPattern(GameManager.Instance.DungeonList[3].Arrow);
        }
        else if (_dungeonLevel % 5 == 0)
        {
            //엘리트몬스터
            SpawnPattern(GameManager.Instance.DungeonList[2].Arrow);
        }
        else
        {
            //일반몬스터
            SpawnPattern(GameManager.Instance.DungeonList[1].Arrow);
        }
    }

    private void SpawnPattern(int arrowCount)
    {
        for (var i = 0; i < arrowCount; i++)
        {
            var pattern = ObjectPool.Instance.GetPooledObject("Pattern");
            pattern.transform.parent = patternBox.transform;

            var arrow = Random.Range(1, 5);
            _pattern.Add(arrow);

            pattern.GetComponent<Image>().sprite = arrowSpriteList[arrow - 1];
        }
    }

    public void PressUp()
    {
        if (_isMonsterTimerOn)
        {
            ButtonInputSystem(EArrow.Up);
        }
    }

    public void PressRight()
    {
        if (_isMonsterTimerOn)
        {
            ButtonInputSystem(EArrow.Right);
        }
    }

    public void PressLeft()
    {
        if(_isMonsterTimerOn)
        {
            ButtonInputSystem(EArrow.Left);
        }
    }

    public void PressDown()
    {
        if(_isMonsterTimerOn)
        {
            ButtonInputSystem(EArrow.Down);
        }
    }

    private void ButtonInputSystem(EArrow eArrow)
    {
        if (_pattern[0] == (int)eArrow)
        {
            //정답
            _pattern.RemoveAt(0);

            var arrow = ObjectPool.Instance.GetPooledObject("Pattern");
            arrow.transform.parent = playerInputBox.transform;
            arrow.GetComponent<Image>().sprite = arrowSpriteList[(int)eArrow - 1];

            if (_pattern.Count == 0)
            {
                //승리
                if (_dungeonLevel % 10 == 0)
                {
                    //보스몬스터
                    GameManager.Instance.Gold += (int)(_enterPower * GameManager.Instance.DungeonList[3].Reward);
                    GameManager.Instance.Stress += 5;
                }
                else if (_dungeonLevel % 5 == 0)
                {
                    //엘리트몬스터
                    GameManager.Instance.Gold += (int)(_enterPower * GameManager.Instance.DungeonList[2].Reward);
                    GameManager.Instance.Stress += 3;
                }
                else
                {
                    //일반몬스터
                    GameManager.Instance.Gold += (int)(_enterPower * GameManager.Instance.DungeonList[1].Reward);
                    GameManager.Instance.Stress += 1;
                }

                _dungeonLevel++;
                
                NextMonster();
            }
        }
        else
        {
            //오답
            NextMonster();
        }
    }

    private void NextMonster()
    {
        _pattern.Clear();
            
        ClearPattern();
            
        SpawnMonster();

        _monsterTime = DefaultMonsterTime;
    }

    private void ClearPattern()
    {
        var patternBoxCount = patternBox.transform.childCount;
        for (var i = 0; i < patternBoxCount; i++)
        {
            var pattern = patternBox.transform.GetChild(0);
            ObjectPool.Instance.ReleaseObjectToPool(pattern.gameObject);
        }

        var playerInputBoxCount = playerInputBox.transform.childCount;
        for (var i = 0; i < playerInputBoxCount; i++)
        {
            var playerInput = playerInputBox.transform.GetChild(0);
            ObjectPool.Instance.ReleaseObjectToPool(playerInput.gameObject);
        }
    }

    private void Exit()
    {
        _isDungeonTimerOn = false;
        _isMonsterTimerOn = false;
        //Exit
        _closePanel.Close();
    }

    public void Update()
    {
        if (_isDungeonTimerOn)
        {
            _dungeonTime -= Time.deltaTime;
            if (_dungeonTime < 0)
            {
                Exit();
            }
            
            timerText.text = (int)_dungeonTime / 60 + " : " + $"{(int)_dungeonTime % 60:D2}";
        }

        if (_isMonsterTimerOn)
        {
            _monsterTime -= Time.deltaTime;
            if (_monsterTime < 0)
            {
                NextMonster();
            }
            
            monsterTimer.value = _monsterTime;
        }
    }
}
