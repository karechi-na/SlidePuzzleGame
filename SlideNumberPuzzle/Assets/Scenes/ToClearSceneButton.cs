using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ‰æ–Ê‚ÌUIButton‚ğ‰Ÿ‚µ‚½‚Æ‚«Scene‚ğØ‚è‘Ö‚¦‚éˆ—
/// </summary>
public class ToClearSceneButton : MonoBehaviour
{
    //ƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚½‚Æ‚«Scene‚ğØ‚è‘Ö‚¦‚é
    public void OnClicToGameSceneButton()
    {
        //Scene‚ğClearScene‚ÉØ‚è‘Ö‚¦‚é
        SceneManager.LoadScene("ClearScene");
    }
}
