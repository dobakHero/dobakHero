using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlackJackHand : MonoBehaviour
{
    [SerializeField] private Sprite backCard;
    [SerializeField] private GameObject deckObject;
    
    private List<GameObject> _cards;

    private int _aceCount;
    private int _numberSum;

    private void Awake()
    {
        _cards = new List<GameObject>();
    }

    private void OnEnable()
    {
        _cards.Clear();

        _aceCount = 0;
        _numberSum = 0;
    }

    public void BackDraw(Card card)
    {
        if (card.Number == 1)
        {
            _aceCount++;
        }

        if (card.Number > 10)
        {
            _numberSum += 10;
        }
        else if (card.Number == 1)
        {
            _numberSum += 11;
        }
        else
        {
            _numberSum += card.Number;
        }

        var cardObj = ObjectPool.Instance.GetPooledObject("Card");
        cardObj.transform.parent = transform;
        cardObj.transform.localPosition = deckObject.transform.localPosition;
        cardObj.transform.localScale = Vector3.one;
        
        _cards.Add(cardObj);
        
        AlignCard();
    }
    
    public void Draw(Card card)
    {
        if (card.Number == 1)
        {
            _aceCount++;
        }

        if (card.Number > 10)
        {
            _numberSum += 10;
        }
        else if (card.Number == 1)
        {
            _numberSum += 11;
        }
        else
        {
            _numberSum += card.Number;
        }
        
        var cardObj = ObjectPool.Instance.GetPooledObject("Card");
        cardObj.transform.parent = transform;
        cardObj.transform.localPosition = deckObject.transform.localPosition;
        cardObj.transform.localScale = Vector3.one;

        cardObj.GetComponent<ChangeCardInfo>().ChangeInfo(card.Number,card.Mark);
        
        _cards.Add(cardObj);

        AlignCard();
    }

    private void AlignCard()
    {
        if (_cards.Count == 1)
        {
            _cards[0].transform.DOLocalMoveX(0, 0.5f);
        }
        else if (_cards.Count == 2)
        {
            _cards[0].transform.DOLocalMoveX(-200, 0.5f);
            _cards[1].transform.DOLocalMoveX(200, 0.5f);
        }
        else if (_cards.Count == 3)
        {
            _cards[0].transform.DOLocalMoveX(-320, 0.5f);
            _cards[1].transform.DOLocalMoveX(0, 0.5f);
            _cards[2].transform.DOLocalMoveX(320, 0.5f);
        }
        else
        {
            var margin = 640 / (_cards.Count-1);
            for (var i = 0; i < _cards.Count; i++)
            {
                _cards[i].transform.DOLocalMoveX(-320 + i * margin, 0.5f);
            }
        }
    }

    public int SumCard()
    {
        while (_numberSum > 21 && _aceCount > 0)
        {
            _numberSum -= 10;
            _aceCount--;
        }

        if (_numberSum > 21)
        {
            return -1;
        }
        
        return _numberSum;
    }

    public void OpenHiddenCard()
    {
        
    }
}
