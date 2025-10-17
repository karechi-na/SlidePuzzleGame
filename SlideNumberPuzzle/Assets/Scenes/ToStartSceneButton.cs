using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 画面のUIButtonを押したときSceneを切り替える処理
/// </summary>
public class ToStartSceneButton : MonoBehaviour
{
    //右クリックしたときSceneを切り替える
    public void OnClicToStartSceneButton()
    {
        //SceneをStartSceneに切り替える
        SceneManager.LoadScene("StartScene");
        Debug.Log("切り替え");
    }
}
