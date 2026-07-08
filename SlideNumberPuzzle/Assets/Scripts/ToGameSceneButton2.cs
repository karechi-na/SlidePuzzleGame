using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameSceneButton2 : MonoBehaviour
{
    
    public void OnClickToGameSceneButton()
    {
        Debug.Log("jikkou!");
        SceneManager.LoadScene("GameScene");
    }
}
