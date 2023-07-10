using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveHandle : MonoBehaviour
{
    [SerializeField] private GameObject slotMachinePanel;
    private Scrollbar _scrollbar;
    private ManageSlotMachine _manageSlotMachine;
    private void Start()
    {
        _scrollbar = transform.parent.parent.gameObject.GetComponent<Scrollbar>();
        _manageSlotMachine = slotMachinePanel.GetComponent<ManageSlotMachine>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && _scrollbar.value > 0)
        {
            if(_scrollbar.value > 0.9f)
            {
                //선택
                _manageSlotMachine.SelectSlot();
            }
            _scrollbar.interactable = false;
        }

        if (_scrollbar.interactable == false)
        {
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, 0, 0.05f);
        }

        if (_scrollbar.value < 0.01)
        {
            _scrollbar.interactable = true;
            _scrollbar.value = 0;
        }
    }
}
