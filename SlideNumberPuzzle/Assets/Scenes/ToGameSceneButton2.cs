using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameSceneButton2 : MonoBehaviour
{
    
    public void OnClicToGameSceneButton()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
