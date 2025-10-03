using System.Collections;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public float time = 0.0f;//経過時間//GameDirectorスクリプトで使用されている関数
    public int score = 0;//スコア//他のスクリプトで加算されている

    private void Start()
    {
        //DataHolderのオブジェクトを探し、何個あるか調べる
        GameObject[] objects = GameObject.FindGameObjectsWithTag("DataHolder");

        //DataHolderが2個以上あったら自身を消す
        if (objects.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//シーンをまたいでも消えないようにする(シングルトン用など)
    }
}


