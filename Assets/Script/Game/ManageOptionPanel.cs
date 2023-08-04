using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageOptionPanel : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    private void Update()
    {
        if (bgmSlider && bgmAudioSource)
        {
            bgmAudioSource.volume = bgmSlider.value;
        }

        if (effectSlider && effectAudioSource)
        {
            effectAudioSource.volume = effectSlider.value;
        }
        
    }

    public void ShutDown()
    {
        ManageSave.Instance.Saved();
        
#if UNITY_EDITOR        //에디터에서만 실행되는 코드
        UnityEditor.EditorApplication.isPlaying = false;
#else                   //빌드된 게임에서만 실행되는 코드
        Application.Quit();
#endif
    }
}
