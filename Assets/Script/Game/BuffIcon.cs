using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Sprite[] iconSprites;
    [SerializeField] private GameObject buffPage;
    [SerializeField] private TextMeshProUGUI buffText;

    private int _id;
    public int id
    {
        get => _id;

        set
        {
            _id = value;
            
            SetSprite();

            buffText.text = _id.ToString();
        }
    }

    private void OnEnable()
    {
        buffPage.SetActive(false);
    }

    private void SetSprite()
    {
        if (id is 2 or 5 or 10)
        {
            GetComponent<Image>().sprite = iconSprites[0];
        }
        else
        {
            GetComponent<Image>().sprite = iconSprites[1];
        }
    }

    public void ShowBuffPage()
    {
        buffPage.SetActive(true);
    }

    public void ExitBuffPage()
    {
        buffPage.SetActive(false);
    }
}
