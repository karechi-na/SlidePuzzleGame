using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameTime;
    [SerializeField] private TMP_Text gameScore;
    // Start is called before the first frame update
    void Start()
    {
        DataHolder holder = GameObject.Find("DataHolder").GetComponent<DataHolder>();
        gameTime.text = holder.time.ToString();
        gameScore.text = holder.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
