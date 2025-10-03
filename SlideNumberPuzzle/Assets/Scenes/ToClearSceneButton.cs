using UnityEngine;
using UnityEngine.SceneManagement;

public class ToClearSceneButton : MonoBehaviour
{
    [Header("SceneReference")]
    [SerializeField] private SceneReference sceneReference = null;

    /// <summary>
    /// ボタンが押されたらクリアシーンへ
    /// </summary>
    public void OnClicToGameSceneButton()
    {
        SceneManager.LoadScene(sceneReference.EndSceneName);
    }
}
