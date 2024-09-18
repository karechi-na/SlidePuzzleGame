using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStartSceneButton : MonoBehaviour
{
    public void OnClicToStartSceneButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
