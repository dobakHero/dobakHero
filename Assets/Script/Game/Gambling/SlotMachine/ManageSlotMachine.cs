using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSlotMachine : MonoBehaviour
{
    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;
    [SerializeField] private GameObject slot3;

    private bool isSlot1Stop = false;
    private bool isSlot2Stop = false;
    private bool isSlot3Stop = false;

    private MoveImage[] _slot1Children;
    private MoveImage[] _slot2Children;
    private MoveImage[] _slot3Children;

    private void Start()
    {
        _slot1Children = slot1.GetComponentsInChildren<MoveImage>();
        _slot2Children = slot2.GetComponentsInChildren<MoveImage>();
        _slot3Children = slot3.GetComponentsInChildren<MoveImage>();
    }

    private void OnEnable()
    {
        isSlot1Stop = false;
        isSlot2Stop = false;
        isSlot3Stop = false;
    }

    public void SelectSlot()
    {
        if (isSlot1Stop == false)
        {
            isSlot1Stop = true;

            foreach (var slot1Child in _slot1Children)
            {
                slot1Child.Stop();
            }
        }
        else if (isSlot2Stop == false)
        {
            isSlot2Stop = true;

            foreach (var slot2Child in _slot2Children)
            {
                slot2Child.Stop();
            }
        }
        else if (isSlot3Stop == false)
        {
            isSlot3Stop = true;

            foreach (var slot3Child in _slot3Children)
            {
                slot3Child.Stop();
            }
        }
    }
}
