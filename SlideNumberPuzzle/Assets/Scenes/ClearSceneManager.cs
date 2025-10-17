using UnityEngine;
using TMPro;

public class ClearSceneManager : MonoBehaviour
{
    [Header("ゲームクリアにかかった時間")]
    [SerializeField] private TMP_Text gameTime;

    [Header("ゲームスコア")]
    [SerializeField] private TMP_Text gameScore;


    // Start is called before the first frame update
    void Start()
    {
        //　DataHolderオブジェクトを取得して、そこからtimeとscoreを取得して表示する
        DataHolder holder = GameObject.Find("DataHolder").GetComponent<DataHolder>();

        gameTime.text = holder.time.ToString("F1") + "[S]";

        gameScore.text = holder.score.ToString();
    }

}
