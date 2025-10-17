using UnityEngine;

/// <summary>
/// DataHolderを複数作らせず、1つだけ保持するためのスクリプト
/// </summary>
public class DataHolder : MonoBehaviour
{
    //ゲーム開始してからの経過時間
    //GameDirectorスクリプトで使用されている関数
    public float time = 0.0f;
    private int dataHolderNumber = 1;//DataHolderの個数を示す数

    //他のスクリプトで加算されている
    [Header("スコア")]
    public int score = 0;

    /// <summary>
    /// DataHolderの数を確認して、一定の数以上あったら消す
    /// </summary>
    private void Start()
    {
        //DataHolderのオブジェクトを探し、何個あるか調べる
        GameObject[] objects = GameObject.FindGameObjectsWithTag("DataHolder");

        //DataHolderが2個以上あったら自身を消す
        if (objects.Length > dataHolderNumber)
        {
            Destroy(gameObject);
        }
        //シーンをまたいでも消えないようにする(シングルトン用など)
        DontDestroyOnLoad(gameObject);
    }
}


