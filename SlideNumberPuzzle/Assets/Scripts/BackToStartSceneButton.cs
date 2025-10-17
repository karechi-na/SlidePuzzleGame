using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルシーンに戻るボタンの処理
/// </summary>
public class BackToStartSceneButton : MonoBehaviour
{
    [Header("Scene Reference")]
    [SerializeField] private SceneReference sceneReference = null;


    /// <summary>
    /// ボタンがクリックを検知したらタイトルシーンへ遷移
    /// </summary>
    public void OnClickToStartSceneButton()
    {
        SceneManager.LoadScene(sceneReference.TitleSceneName);
    }
}
