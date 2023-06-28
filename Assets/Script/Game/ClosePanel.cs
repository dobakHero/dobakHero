using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
