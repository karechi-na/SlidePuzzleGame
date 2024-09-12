using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Vector2 startPos;
    public bool isTransform = false;
    private GameDirector gameDirector;
    public Vector2 gridPosition =Vector2.zero;

    void Start()
    {
        
    }


    //// Update is called once per frame
    //void Update()
    //{

    //    float posi = 7;
    //    float xPosi = 0;
    //    float yPosi = 0;

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        this.startPos = Input.mousePosition;
    //    }
    //    else if (Input.GetMouseButtonUp(0))
    //    {
    //        //Vector2 endPos = Input.mousePosition;
    //        //xPosi = Mathf.Abs(endPos.x - this.startPos.x);
    //        //yPosi = Mathf.Abs(endPos.y - this.startPos.y);
    //        ////ブロックの座標を取得
    //        //Transform myTransformBefore = this.transform;
    //        //Vector3 posBefore = myTransformBefore.position;

    //        ////マウスの移動した方向にブロックを動かす。
    //        //if (yPosi < xPosi && endPos.x - this.startPos.x < 0)
    //        //{
    //        //    transformLeft(posi, posBefore.y);
    //        //}
    //        //else if (yPosi < xPosi && endPos.x - this.startPos.x > 0)
    //        //{
    //        //    transformRight(posi, posBefore.y);
    //        //}
    //        //else if (xPosi < yPosi && endPos.y - this.startPos.y < 0)
    //        //{
    //        //    transformDown(posi, posBefore.x);
    //        //}
    //        //else if (xPosi < yPosi && endPos.y - this.startPos.y > 0)
    //        //{
    //        //    transformUp(posi, posBefore.x);
    //        //}

    //        //移動前の座標と後の座標を比較して違う場合新しいブロックを生成
    //        Transform myTransformAfter = this.transform;
    //        Vector3 posAfter = myTransformAfter.position;
    //        if (posAfter != posBefore)
    //        {
    //            //BlockGeneratorにisTransformを渡して生成させる
    //            isTransform = true;
    //        }
    //    }

    //    if ()
    //    {

    //    }

    //}



    //左に移動
    public float transformLeft(float leftPosi, float posY)
    {
        leftPosi = leftPosi * -1;
        this.transform.DOMove(new Vector3(1, posY, 0), 0.5f);

        return leftPosi;
    }

    //右に移動
    public void transformRight(int moveGridCount)
    {
        this.transform.DOMove(new Vector3(transform.position.x + (2.0f* moveGridCount), transform.position.y, 0), 0.5f);
    }

    //下に移動
    public float transformDown(float downPosi, float posX)
    {
        downPosi = downPosi * -1;
        this.transform.DOMove(new Vector3(posX, downPosi, 0), 0.5f);

        return downPosi;
    }

    //上に移動
    public float transformUp(float upPosi, float posX)
    {
        this.transform.DOMove(new Vector3(posX, -1, 0), 0.5f);

        return upPosi;
    }
}
