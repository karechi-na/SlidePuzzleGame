using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1728, 1080, false);
    }
    private void Update()
    {
        Debug.Log("ñ‚ëËÇ»ÇµÅI");
    }
}
