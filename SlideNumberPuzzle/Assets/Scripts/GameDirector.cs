using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private GameObject SquareNo2;
    private BlockGenerator generator;
    private int keepX = 0;
    private int keepY = 0;
    private int count = 0;
    public int xCoordinate = 0;
    public int yCoordinate = 0;
    private Vector2 startPos;
    private List<BlockController> blockControllerList = new List<BlockController>();

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        //開始時のブロック二つを生成
        for (; ; )
        {
            xCoordinate = Random.Range(0, 8);
            yCoordinate = Random.Range(0, 8);

            //生成用の変数
            int vertical = yCoordinate * -1;
            int horizontal = xCoordinate;

            CompareDirectionX(xCoordinate);
            CompareDirectionY(yCoordinate);

            if (keepX != horizontal && keepY != vertical && horizontal % 2 != 0 && vertical % 2 != 0)
            {
                GameObject square = Instantiate(SquareNo2);
                square.transform.position = new Vector3(horizontal, vertical, 0);
                BlockController bc = square.GetComponent<BlockController>();
                bc.gridPosition = new Vector2(Mathf.Floor(xCoordinate / 2.0f), Mathf.Floor(yCoordinate / 2.0f));
                blockControllerList.Add(bc);
                keepX = horizontal;
                keepY = vertical;
                count++;
                //array2D[xCoordinate, yCoordinate] = true;
            }
            if (count == 2)
            {
                break;
            }
        }


    }

    private void Update()
    {
        float posi = 7;
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
            Transform myTransformBefore = this.transform;
            Vector3 posBefore = myTransformBefore.position;

            //マウスの移動した方向にブロックを動かす。
            if (yPosi < xPosi && endPos.x - this.startPos.x < 0)
            {
                MoveLeft();
            }
            else if (yPosi < xPosi && endPos.x - this.startPos.x > 0)
            {
                MoveRight();
            }
            else if (xPosi < yPosi && endPos.y - this.startPos.y < 0)
            {
                MoveDown();
            }
            else if (xPosi < yPosi && endPos.y - this.startPos.y > 0)
            {
               MoveUp();
            }
        }
    }



    //右に移動させるメソッド
    private void MoveRight()
    {
        foreach (BlockController bc in blockControllerList)
        {
            if (bc.gridPosition.x < 3)
            {
                int count = 0;
                foreach (BlockController checkBc in blockControllerList)
                {
                    if (bc.gridPosition.y == checkBc.gridPosition.y && bc.gridPosition.x < checkBc.gridPosition.x)
                    {
                        count++;
                    }
                }

                if (bc.gridPosition.x < 3 - count)
                {
                    bc.transformRight((int)((3 - count) - bc.gridPosition.x));
                }
            }
        }
    }

    //左に移動させるメソッド
    private void MoveLeft()
    {
        foreach (BlockController bc in blockControllerList)
        {
            if (bc.gridPosition.x > 0)
            {
                int count = 0;
                foreach (BlockController checkBc in blockControllerList)
                {
                    if (bc.gridPosition.y == checkBc.gridPosition.y && bc.gridPosition.x > checkBc.gridPosition.x)
                    {
                        count++;
                    }
                }
                if (bc.gridPosition.x > count)
                {
                    bc.transformLeft((int) bc.gridPosition.x - count);
                }
            }
        }
    }

    //下に移動させるメソッド
    private void MoveDown()
    {
        foreach (BlockController bc in blockControllerList)
        {
            if (bc.gridPosition.y < 3)
            {
                int count = 0;
                foreach (BlockController checkBc in blockControllerList)
                {
                    if (bc.gridPosition.x == checkBc.gridPosition.x && bc.gridPosition.y < checkBc.gridPosition.y)
                    {
                        count++;
                    }
                }
                if (bc.gridPosition.y < 3 - count)
                {
                    bc.transformDown((int)((3 - count) - bc.gridPosition.y));
                }
            }
        }
    }

    //上に移動させるメソッド
    private void MoveUp()
    {
        foreach (BlockController bc in blockControllerList)
        {
            if (bc.gridPosition.y > 0)
            {
                int count = 0;
                foreach (BlockController checkBc in blockControllerList)
                {
                    if (bc.gridPosition.x == checkBc.gridPosition.x && bc.gridPosition.y > checkBc.gridPosition.y)
                    {
                        count++;
                    }
                }
                if (bc.gridPosition.y > count)
                {
                    bc.transformUp((int)bc.gridPosition.y - count);
                }
            }
        }
    }

    public int CompareDirectionX(int xCoordinate)
    {
        if (xCoordinate == 1)
        {
            xCoordinate = 0;
        }
        else if (xCoordinate == 3)
        {
            xCoordinate = 1;
        }
        else if (xCoordinate == 5)
        {
            xCoordinate = 2;
        }
        else if (xCoordinate == 7)
        {
            xCoordinate = 3;
        }

        return xCoordinate;
    }

    public int CompareDirectionY(int yCoordinate)
    {
        if (yCoordinate == 1)
        {
            yCoordinate = 0;
        }
        else if (yCoordinate == 3)
        {
            yCoordinate = 1;
        }
        else if (yCoordinate == 5)
        {
            yCoordinate = 2;
        }
        else if (yCoordinate == 7)
        {
            yCoordinate = 3;
        }

        return yCoordinate;
    }
}
