using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEcontroller : MonoBehaviour
{
    public AudioClip SE;
    AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        this.aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
