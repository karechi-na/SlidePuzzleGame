using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;

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
