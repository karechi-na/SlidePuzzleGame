using UnityEngine;
using TMPro;

public class ClearSceneManager : MonoBehaviour
{
    [Header("")]
    [SerializeField] private TMP_Text gameTime;

    [Header("")]
    [SerializeField] private TMP_Text gameScore;


    // Start is called before the first frame update
    void Start()
    {
        DataHolder holder = GameObject.Find("DataHolder").GetComponent<DataHolder>();
        gameTime.text = holder.time.ToString("F1") + "[S]";
        gameScore.text = holder.score.ToString();
    }

}
