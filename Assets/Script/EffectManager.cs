using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    
    [SerializeField] private List<AudioClip> effectList;

    private AudioSource _audioSource;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayDobakDefeat()
    {
        _audioSource.clip = effectList.Find(x => x.name == "Dobak_Defeat");
        _audioSource.Play();
    }

    public void PlayDobakVictory()
    {
        _audioSource.clip = effectList.Find(x => x.name == "Dobak_Victory");
        _audioSource.Play();
    }
    
    public void PlayEventNegative()
    {
        _audioSource.clip = effectList.Find(x => x.name == "Event_Negative");
        _audioSource.Play();
    }
    
    public void PlayEventPositive()
    {
        _audioSource.clip = effectList.Find(x => x.name == "Event_Positive");
        _audioSource.Play();
    }
    
    public void PlayMonsterBoss()
    {
        _audioSource.clip = effectList.Find(x => x.name == "Monster_Boss");
        _audioSource.Play();
    }
    
    public void PlayMonsterElite()
    {
        _audioSource.clip = effectList.Find(x => x.name == "Monster_Elite");
        _audioSource.Play();
    }
    
    public void PlayMonsterNormal()
    {
        _audioSource.clip = effectList.Find(x => x.name == "Monster_Normal");
        _audioSource.Play();
    }
}
