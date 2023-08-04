using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ManageBlackJack : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chipCountText;
    [SerializeField] private GameObject bettingPanel;

    [SerializeField] private BlackJackHand userHand;
    [SerializeField] private BlackJackHand dealerHand;

    [SerializeField] private Button drawBtn;
    [SerializeField] private Button gameEndBtn;

    [SerializeField] private ManageRoulette manageRoulette;
    
    private Card[] _deck;

    private int _chipCount;

    private int _deckIdx;

    private void Awake()
    {
        _deck = new Card[52];
        
        var idx = 0;
        for (var i = 0; i < 4; i++)
        {
            var mark = i switch
            {
                0 => EMark.Spade,
                1 => EMark.Heart,
                2 => EMark.Clover,
                _ => EMark.Diamond
            };
            for (var j = 1; j < 14; j++)
            {
                _deck[idx] = new Card(j, mark);
                idx++;
            }
        }
    }

    private void OnEnable()
    {
        _chipCount = 1;
        _deckIdx = 0;

        bettingPanel.SetActive(true);

        var userHandCnt = userHand.transform.childCount - 1;
        for (var i = 0; i < userHandCnt; i++)
        {
            ObjectPool.Instance.ReleaseObjectToPool(userHand.transform.GetChild(1).gameObject);
        }

        var dealerHandCnt = dealerHand.transform.childCount - 1;
        for (var i = 0; i < dealerHandCnt; i++)
        {
            ObjectPool.Instance.ReleaseObjectToPool(dealerHand.transform.GetChild(1).gameObject);
        }
        
        drawBtn.interactable = true;
        gameEndBtn.interactable = true;
    }

    public void UpChipCount()
    {
        if (_chipCount >= GameManager.Instance.Chip)
            return;
        
        _chipCount++;
        chipCountText.text = _chipCount.ToString();
    }

    public void DownChipCount()
    {
        if (_chipCount <= 1)
            return;

        _chipCount--;
        chipCountText.text = _chipCount.ToString();
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

    public void GameStart()
    {
        if (_chipCount > GameManager.Instance.Chip)
            return;
        
        bettingPanel.SetActive(false);

        GameManager.Instance.Chip -= _chipCount;
        GameManager.Instance.Stress += GameManager.Instance.GamblingList[3].Stress;

        GameManager.Instance.gamblingCurrentCount -= 1;

        ShuffleArray(_deck);

        dealerHand.BackDraw(_deck[_deckIdx++]);
        dealerHand.Draw(_deck[_deckIdx++]);

        Debug.Log("dealer : " + dealerHand.SumCard());
        
        userHand.Draw(_deck[_deckIdx++]);
        userHand.Draw(_deck[_deckIdx++]);
        
        Debug.Log("user : " + userHand.SumCard());

        if (userHand.SumCard() == 21 || dealerHand.SumCard() == 21)
        {
            GameEnd();
        }
    }

    public void GameEnd()
    {
        if (userHand.SumCard() == dealerHand.SumCard())
        {
            GameManager.Instance.Chip += _chipCount;
        }
        else if (userHand.SumCard() > dealerHand.SumCard())
        {
            if (userHand.SumCard() == 21)
            {
                var reward = (int)(_chipCount * 2.5f);
                
                //선조의 축복
                reward = (int)(reward * (1 + GameManager.Instance.blessChipLevel * GameManager.Instance.BlessList[1].Amount));
                
                //돼지머리
                if (GameManager.Instance.hasPighead)
                    reward = (int)(reward * (1 + GameManager.Instance.ShopList[5].Reward));

                GameManager.Instance.Chip += reward;
            }
            else
            {
                var reward = (int)(_chipCount * 2f);
                
                //선조의 축복
                reward = (int)(reward * (1 + GameManager.Instance.blessChipLevel * GameManager.Instance.BlessList[1].Amount));
                
                //돼지머리
                if (GameManager.Instance.hasPighead)
                    reward = (int)(reward * (1 + GameManager.Instance.ShopList[5].Reward));

                GameManager.Instance.Chip += reward;
            }
            
            EffectManager.Instance.PlayDobakVictory();
        }
        else
        {
            EffectManager.Instance.PlayDobakDefeat();
        }
        
        dealerHand.OpenHiddenCard();
        
        drawBtn.interactable = false;
        gameEndBtn.interactable = false;
    }

    public void DrawCard()
    {
        userHand.Draw(_deck[_deckIdx++]);
        
        Debug.Log("user : " + userHand.SumCard());

        if (userHand.SumCard() == 21 || userHand.SumCard() == -1)
        {
            GameEnd();
        }
    }
}
