using UnityEngine;

/// <summary>
/// BGMを管理するクラス
/// </summary>
public class MusicController : MonoBehaviour
{
    [Header("鳴らす音")]
    [SerializeField] private AudioClip sound1;

    //AudioSourceの情報
    private AudioSource audioSource;

    void Start()
    {
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //マウスの左ボタンを離したとき
        if (Input.GetMouseButtonUp(0))
        {
            //音(sound1)を鳴らす
            audioSource.PlayOneShot(sound1);
        }
    }
}
