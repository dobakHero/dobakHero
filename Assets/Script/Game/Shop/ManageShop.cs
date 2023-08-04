using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageShop : MonoBehaviour
{
    [SerializeField] private ShopItem[] items;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private ManageItemPanel itemPanel;

    private void Awake()
    {
        for (var i = 0; i < items.Length; i++)
        {
            items[i].SetSprite(sprites[i]);
        }
    }

    private void OnEnable()
    {
        itemPanel.gameObject.SetActive(false);
    }

    public void OpenItemPanel(int id)
    {
        itemPanel.SetItem(id, sprites[id - 1],GameManager.Instance.ScriptList["Script_"+items[id-1].name]);
    }
}
