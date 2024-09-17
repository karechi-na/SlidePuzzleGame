using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameSceneButton : MonoBehaviour
{
    public void OnClikToGameSceneButton()
    {
        SceneManager.LoadScene("GameScene.taku");
    }
    
}
