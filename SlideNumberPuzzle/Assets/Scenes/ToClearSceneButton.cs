using UnityEngine;
using UnityEngine.SceneManagement;

public class ToClearSceneButton : MonoBehaviour
{
    public void OnClicToGameSceneButton()//ƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚½‚Æ‚«Scene‚ğØ‚è‘Ö‚¦‚é
    {
        SceneManager.LoadScene("ClearScene");//Scene‚ğClearScene‚ÉØ‚è‘Ö‚¦‚é
    }
}
