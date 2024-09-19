using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public float time = 0.0f;
    public int score = 0;



    private void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("DataHolder");

        if(objects.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}


