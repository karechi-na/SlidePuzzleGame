using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStartSceneButton : MonoBehaviour
{
    public void OnClicToStartSceneButton()//右クリックしたときSceneを切り替える
    {
        SceneManager.LoadScene("StartScene");//SceneをStartSceneに切り替える
        Debug.Log("切り替え");
    }
}
