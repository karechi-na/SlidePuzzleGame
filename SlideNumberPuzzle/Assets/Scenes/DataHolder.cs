using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : SingletonMonobehaviour<DataHolder>
{
    public float time = 0.0f;
    public int score = 0;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// ‰Šú‰»ƒƒ\ƒbƒh
    /// </summary>
    public void DataInitialization()
    {
        time = 0.0f;
        score = 0;
    }
}


