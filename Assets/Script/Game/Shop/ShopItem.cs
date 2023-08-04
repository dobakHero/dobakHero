using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImg;

    private Shop _item;
    private void Start()
    {
        foreach (var item in GameManager.Instance.ShopList)
        {
            if (item.Value.English == transform.name)
            {
                _item = item.Value;
                break;
            }
        }

        itemName.text = _item.Korean;
    }

    public void SetSprite(Sprite sprite)
    {
        itemImg.sprite = sprite;
    }
}
