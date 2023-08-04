using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageItemPanel : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemScript;
    [SerializeField] private ManageGamePanel gamePanel;

    private int _itemID;

    public void SetItem(int id, Sprite sprite, string script)
    {
        _itemID = id;

        itemImage.sprite = sprite;
        itemScript.text = script;
        
        gameObject.SetActive(true);
    }

    public void BuyItem()
    {
        if (GameManager.Instance.Gold < GameManager.Instance.ShopList[_itemID].Price)
            return;
        
        switch (_itemID)
        {
            case 1:
                GetCure();
                break;
            case 2:
                if (!GetEnhance())
                    return;
                break;
            case 3:
                if (!GetFigure())
                    return;
                break;
            case 4:
                GetCigarette();
                break;
            case 5:
                if (!GetPighead())
                    return;
                break;
            case 6:
                if (!GetSteak())
                    return;
                break;
            case 7:
                if (!GetBeer())
                    return;
                break;
            case 8:
                if (!GetCake())
                    return;
                break;
        }
        
        ManageSave.Instance.Saved();
        
        gameObject.SetActive(false);
    }

    private void GetCure()
    {
        //게임 승리
    }

    private bool GetEnhance()
    {
        //전투력 측정
        if(GameManager.Instance.Power >= 100000)
            return false;

        //가격측정
        var price = GameManager.Instance.Power / 10000 + 1;
        price *= 10000;

        if (price > GameManager.Instance.Gold)
            return false;
        
        //구매
        GameManager.Instance.Gold -= price;
        GameManager.Instance.Power += GameManager.Instance.ShopList[2].Power;

        //전투력 초과 측정
        if (GameManager.Instance.Power > 100000) 
            GameManager.Instance.Power = 100000;

        return true;
    }

    private bool GetFigure()
    {
        //재고확인
        if (GameManager.Instance.figureStock == 0)
            return false;

        var price = GameManager.Instance.ShopList[3].Price;

        //가격측정
        if (GameManager.Instance.figureStock == 1)
            price *= 10;

        if (GameManager.Instance.Gold < price)
            return false;

        GameManager.Instance.Gold -= price;
        GameManager.Instance.gamblingMaxCount += 1;
        GameManager.Instance.figureStock -= 1;

        return true;
    }

    private void GetCigarette()
    {
        GameManager.Instance.Gold -= GameManager.Instance.ShopList[4].Price;
        GameManager.Instance.Power += GameManager.Instance.ShopList[4].Power;
        GameManager.Instance.Stress += GameManager.Instance.ShopList[4].Stress;

        if (GameManager.Instance.Power < 5000)
            GameManager.Instance.Power = 5000;
    }

    private bool GetPighead()
    {
        if (GameManager.Instance.AppliedEvent[9].IsActive || GameManager.Instance.hasPighead)
            return false;


        GameManager.Instance.Gold -= GameManager.Instance.ShopList[5].Price;
        GameManager.Instance.hasPighead = true;
        
        return true;
    }

    private bool GetSteak()
    {
        if (GameManager.Instance.AppliedEvent[9].IsActive || GameManager.Instance.hasSteak)
            return false;

        GameManager.Instance.Gold -= GameManager.Instance.ShopList[6].Price;
        GameManager.Instance.Stress += GameManager.Instance.ShopList[6].Stress;

        GameManager.Instance.hasSteak = true;
        GameManager.Instance.PowerMultipleList.Add("Steak", GameManager.Instance.ShopList[6].Reward);

        return true;
    }

    private bool GetBeer()
    {
        if (GameManager.Instance.AppliedEvent[9].IsActive)
            return false;

        GameManager.Instance.Gold -= GameManager.Instance.ShopList[7].Price;
        GameManager.Instance.Stress += GameManager.Instance.ShopList[7].Stress;
        
        gamePanel.NextDay();

        return true;
    }

    private bool GetCake()
    {
        if (GameManager.Instance.AppliedEvent[9].IsActive)
            return false;

        GameManager.Instance.Gold -= GameManager.Instance.ShopList[8].Price;
        GameManager.Instance.Stress += GameManager.Instance.ShopList[8].Stress;

        return true;
    }
}
