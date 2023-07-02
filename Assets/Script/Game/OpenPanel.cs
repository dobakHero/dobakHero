using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject storyPanel;

    private void Start()
    {
        if (storyPanel)
        {
            storyPanel.SetActive(true);
        }
    }

    public void OpenEventPanel()
    {
        if (eventPanel)
        {
            eventPanel.SetActive(true);
        }
    }

    public void OpenOptionPanel()
    {
        if (optionPanel)
        {
            optionPanel.SetActive(true);
        }
    }
}
