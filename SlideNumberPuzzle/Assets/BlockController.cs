using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{

    private Vector2 startPos;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float posi = 3;
        float xPosi = 0;
        float yPosi = 0;

        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            xPosi = Mathf.Abs(endPos.x - this.startPos.x);
            yPosi = Mathf.Abs(endPos.y - this.startPos.y);
            //ブロックの座標を取得
            Transform myTransform = this.transform;
            Vector3 pos = myTransform.position;

            //マウスの移動した方向にブロックを動かす。
            if (yPosi < xPosi && endPos.x - this.startPos.x < 0)
            {
                transformLeft(posi, pos.y);
            }
            else if (yPosi < xPosi && endPos.x - this.startPos.x > 0)
            {
                transformRight(posi, pos.y);
            }
            else if (xPosi < yPosi && endPos.y - this.startPos.y < 0)
            {
                transformDown(posi, pos.x);
            }
            else if (xPosi < yPosi && endPos.y - this.startPos.y > 0)
            {
                transformUp(posi, pos.x);
            }

        }
    }

    //左に移動
    private float transformLeft(float leftPosi, float posY)
    {
        leftPosi = leftPosi * -1;
        this.transform.DOMove(new Vector3(leftPosi, posY, 0), 0.5f);

        return leftPosi;
    }

    //右に移動
    private float transformRight(float rightPosi, float posY)
    {
        this.transform.DOMove(new Vector3(rightPosi, posY, 0), 0.5f);

        return rightPosi;
    }

    //下に移動
    private float transformDown(float downPosi, float posX)
    {
        downPosi = downPosi * -1;
        this.transform.DOMove(new Vector3(posX, downPosi, 0), 0.5f);

        return downPosi;
    }

    //上に移動
    private float transformUp(float upPosi, float posX)
    {
        this.transform.DOMove(new Vector3(posX, upPosi, 0), 0.5f);

        return upPosi;
    }
}
