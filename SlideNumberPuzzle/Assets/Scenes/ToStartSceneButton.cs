using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStartSceneButton : MonoBehaviour
{
    [Header("SceneReference")]
    [SerializeField] private SceneReference sceneReference = null;

    /// <summary>
    /// ボタンが押されたらタイトルシーンへ
    /// </summary>
    public void OnClicToStartSceneButton()
    {
        SceneManager.LoadScene(sceneReference.TitleSceneName);
        Debug.Log("切り替え");
    }
}
