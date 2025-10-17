using UnityEngine;

/// <summary>
/// ゲーム開始時画面解像度を設定するスクリプト
/// </summary>
public class ScreenManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1728, 1080, false);//画面解像度を1728x1080に設定
    }
}
