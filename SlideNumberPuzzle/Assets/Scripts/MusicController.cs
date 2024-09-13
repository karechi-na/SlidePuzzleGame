using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;

    void Start()
    {
        //Component‚ðŽæ“¾
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            //‰¹(sound1)‚ð–Â‚ç‚·
            audioSource.PlayOneShot(sound1);
        }
    }
}
