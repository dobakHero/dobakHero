using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private GameObject gamblingPanel;
    [SerializeField] private GameObject dungeonPanel;
    [SerializeField] private GameObject shopPanel;

    private ManageDungeonPanel _manageDungeonPanel;

    private void Start()
    {
        if (storyPanel)
        {
            storyPanel.SetActive(true);
        }
        
        if (optionPanel)
        {
            optionPanel.SetActive(false);
        }

        if (gamblingPanel)
        {
            gamblingPanel.SetActive(false);
        }

        if (dungeonPanel)
        {
            dungeonPanel.SetActive(false);
        }

        if (shopPanel)
        {
            shopPanel.SetActive(false);
        }
    }

    public void OpenOptionPanel()
    {
        if (optionPanel)
        {
            optionPanel.SetActive(true);
        }
    }

    public void OpenGamblingPanel()
    {
        if (gamblingPanel)
        {
            gamblingPanel.SetActive(true);
        }
    }

    public void OpenDungeonPanel()
    {
        if (dungeonPanel && GameManager.Instance.canEnterDungeon)
        {
            dungeonPanel.SetActive(true);

            if (_manageDungeonPanel == null)
            {
                _manageDungeonPanel = dungeonPanel.GetComponent<ManageDungeonPanel>();
            }
            
            _manageDungeonPanel.EnterDungeon();
        }
    }

    public void OpenShopPanel()
    {
        if (shopPanel)
        {
            shopPanel.SetActive(true);
        }
    }
}
