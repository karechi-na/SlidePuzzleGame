using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStartScene : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // マウスの左ボタンを押したとき
        if (Input.GetMouseButtonDown(0))
        {
            // StartSceneに遷移する
            SceneManager.LoadScene("StartScene");
        }
    }
}
