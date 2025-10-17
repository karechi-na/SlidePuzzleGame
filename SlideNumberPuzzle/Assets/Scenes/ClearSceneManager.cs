using UnityEngine;
using TMPro;
/// <summary>
/// ClearSceneでクリア時の時間とスコアを表示するスクリプト
/// </summary>
public class ClearSceneManager : MonoBehaviour
{
    //Time表示用テキスト
    [SerializeField] private TMP_Text gameTime;

    //Score表示用テキスト
    [SerializeField] private TMP_Text gameScore;

    /// <summary>
    /// ゲーム開始時DataHolderスクリプトから時間とスコアを取得して表示する
    /// </summary>
    public void Start()
    {
        //DataHolderとゆうスクリプトを呼び出す
        DataHolder holder = GameObject.Find("DataHolder").GetComponent<DataHolder>();

        //テキスト表示
        gameTime.text = holder.time.ToString("F1") + "[S]";

        //スコア表示
        gameScore.text = holder.score.ToString();
    }
}
