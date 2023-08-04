using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManageGameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        text.text = "획득 가능한 선조의 축복 : " + GameManager.Instance.Level / 10;
        ManageSave.Instance.Saved();
    }

    public void ReStart()
    {
        GameManager.Instance.Bless += GameManager.Instance.Level / 10;
        
        GameManager.Instance.ReStart();
        
        OpenPanel.Instance.ReSetPanel();
        
        ManageSave.Instance.Saved();
    }
}
