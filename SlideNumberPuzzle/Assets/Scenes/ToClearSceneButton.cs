using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToClearSceneButton : MonoBehaviour
{
    public void OnClicToGameSceneButton()
    {
        SceneManager.LoadScene("ClearScene");
    }
}
