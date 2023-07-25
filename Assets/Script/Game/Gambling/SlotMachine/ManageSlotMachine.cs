using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSlotMachine : MonoBehaviour
{
    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;
    [SerializeField] private GameObject slot3;

    private bool _isSlot1Stop = false;
    private bool _isSlot2Stop = false;
    private bool _isSlot3Stop = false;

    private MoveImage[] _slot1Children;
    private MoveImage[] _slot2Children;
    private MoveImage[] _slot3Children;

    private int _slot1Idx;
    private int _slot2Idx;
    private int _slot3Idx;
    
    private void Start()
    {
        _slot1Children = slot1.GetComponentsInChildren<MoveImage>();
        _slot2Children = slot2.GetComponentsInChildren<MoveImage>();
        _slot3Children = slot3.GetComponentsInChildren<MoveImage>();
    }

    private void OnEnable()
    {
        _isSlot1Stop = false;
        _isSlot2Stop = false;
        _isSlot3Stop = false;
    }

    public void SelectSlot()
    {
        if (_isSlot1Stop == false)
        {
            if (GameManager.Instance.Chip < GameManager.Instance.GamblingList[1].Cost)
            {
                //게임을 진행할 수 없습니다.
                
                return;
            }
            
            GameManager.Instance.Chip -= GameManager.Instance.GamblingList[1].Cost;
            
            GameManager.Instance.gamblingCurrentCount -= 1;
            
            _isSlot1Stop = true;

            foreach (var slot1Child in _slot1Children)
            {
                slot1Child.Stop();

                if (slot1Child.IsSelected())
                {
                    _slot1Idx = slot1Child.GetCurImgIdx();
                }
            }
        }
        else if (_isSlot2Stop == false)
        {
            _isSlot2Stop = true;

            foreach (var slot2Child in _slot2Children)
            {
                slot2Child.Stop();
                
                if (slot2Child.IsSelected())
                {
                    _slot2Idx = slot2Child.GetCurImgIdx();
                }
            }
        }
        else if (_isSlot3Stop == false)
        {
            _isSlot3Stop = true;

            foreach (var slot3Child in _slot3Children)
            {
                slot3Child.Stop();
                
                if (slot3Child.IsSelected())
                {
                    _slot3Idx = slot3Child.GetCurImgIdx();
                }
            }
            
            //끝
            GameManager.Instance.Stress += GameManager.Instance.GamblingList[1].Stress;

            if (_slot1Idx == 4 && _slot2Idx == 4 && _slot3Idx == 4)
            {
                //777
                GameManager.Instance.Chip += 1000;
            }
            else if (_slot1Idx == _slot2Idx && _slot2Idx == _slot3Idx)
            {
                GameManager.Instance.Chip += GameManager.Instance.GamblingList[1].Reward;
            }
        }
    }
}
