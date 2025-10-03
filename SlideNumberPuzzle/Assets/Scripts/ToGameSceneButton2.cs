using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameSceneButton2 : MonoBehaviour
{
    [Header("Scene Reference")]
    [SerializeField] private SceneReference sceneReference = null;

    /// <summary>
    /// ボタンが押されたときにゲームシーンに遷移する
    /// </summary>
    public void OnClicToGameSceneButton()
    {
        SceneManager.LoadScene(sceneReference.SceneName);
    }
}
