using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public float time = 0.0f;
    public int score = 0;


    public void Awake()
    {
        time = 0.0f;
        score = 0;

    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}


