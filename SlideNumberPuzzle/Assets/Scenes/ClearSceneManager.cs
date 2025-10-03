using UnityEngine;
using TMPro;

public class ClearSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameTime;//Time表示用テキスト
    [SerializeField] private TMP_Text gameScore;//Score表示用テキスト
    // Start is called before the first frame update
    public void Start()
    {
        //DataHolderとゆうスクリプトを呼び出す
        DataHolder holder = GameObject.Find("DataHolder").GetComponent<DataHolder>();
        gameTime.text = holder.time.ToString("F1") + "[S]";//テキスト表示
        gameScore.text = holder.score.ToString();//スコア表示
    }
}
