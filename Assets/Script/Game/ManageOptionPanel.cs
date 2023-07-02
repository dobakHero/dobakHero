using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageOptionPanel : MonoBehaviour
{
    public void ShutDown()
    {
#if UNITY_EDITOR        //에디터에서만 실행되는 코드
        UnityEditor.EditorApplication.isPlaying = false;
#else                   //빌드된 게임에서만 실행되는 코드
        Application.Quit();
#endif
    }
}
