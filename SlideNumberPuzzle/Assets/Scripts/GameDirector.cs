using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private GameObject SquareNo2;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    //private BlockController blockUnion;
    //private BlockGenerator generator;
    private int keepX = 0;
    private int keepY = 0;
    private int count = 0;
    private float actionTime = 0;
    public int xCoordinate = 0;
    public int yCoordinate = 0;
    public bool isSE = false;
    private Vector2 startPos;
    public List<BlockController> blockControllerList = new List<BlockController>();
    public static GameDirector gameDirector;
    private DataHolder dataHolder;

    private bool[,] isFieldActive = new bool[,]
    {
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
    };

    // Start is called before the first frame update
    void Start()
    {
        dataHolder = GameObject.Find("DataHolder").GetComponent<DataHolder>();
        Application.targetFrameRate = 60;

        dataHolder.time = 0;
        dataHolder.score = 0;

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
                bc.gridPosition = new Vector2(Mathf.Abs(Mathf.Floor(xCoordinate / 2.0f)), Mathf.Floor(yCoordinate / 2.0f));
                blockControllerList.Add(bc);

                keepX = horizontal;
                keepY = vertical;
                count++;
            }
            if (count == 2)
            {
                break;
            }
        }
    }

    private void Update()
    {
        float xPosi = 0;
        float yPosi = 0;

        dataHolder.time += Time.deltaTime;
        actionTime += Time.deltaTime;


        timeText.text = dataHolder.time.ToString("F1") + "[s]";

        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            xPosi = Mathf.Abs(endPos.x - this.startPos.x);
            yPosi = Mathf.Abs(endPos.y - this.startPos.y);
            if(actionTime >= 0.5f)
            {
                //マウスの移動した方向にブロックを動かす。
                if (yPosi < xPosi && endPos.x - this.startPos.x < 0)
                {
                    MoveLeft();
                    actionTime = 0;
                }
                else if (yPosi < xPosi && endPos.x - this.startPos.x > 0)
                {
                    MoveRight();
                    actionTime = 0;
                }
                else if (xPosi < yPosi && endPos.y - this.startPos.y < 0)
                {
                    MoveDown();
                    actionTime = 0;
                }
                else if (xPosi < yPosi && endPos.y - this.startPos.y > 0)
                {
                    MoveUp();
                    actionTime = 0;
                }
            }
            
        }
        //if ()
        //{
        //    SceneManager.LoadScene("ClearScene");
        //}
    }

    private void RefleshFieldActiveList()
    {
        for (var x = 0; x < 4; x++)
        {
            for (var y = 0; y < 4; y++)
            {
                isFieldActive[x, y] = false;
            }
        }
    }

    private void CreateNewBlock()
    {
        var emptyPositionList = new List<Vector2>();
        for (var x = 0; x < 4; x++)
        {
            for (var y = 0; y < 4; y++)
            {
                if (!isFieldActive[x, y])
                {
                    emptyPositionList.Add(new Vector2(x, y));
                }
            }
        }

        if (emptyPositionList.Count != 0)
        {
            var random = Random.Range(0, emptyPositionList.Count);
            var createPos = emptyPositionList[random];

            GameObject square = Instantiate(SquareNo2);
            square.transform.position = new Vector3(createPos.x * 2.0f + 1.0f, -createPos.y * 2.0f - 1.0f, 0); ;

            BlockController bc = square.GetComponent<BlockController>();
            bc.gridPosition = createPos;
            blockControllerList.Add(bc);
        }
    }

    //右に移動させるメソッド
    private void MoveRight()
    {
        RefleshFieldActiveList();
        bool isMoveBlock = false;
        for (int x = 3; x >= 0; x--)
        {
            for (int y = 0; y < 4; y++)
            {
                var gridPos = new Vector2(x, y);
                var bc = CheckBlockController(gridPos);
                if (bc == null)
                {
                    continue;
                }

                // 移動先検索
                int count = 0;
                bool isNear = false;
                for (var check_x = x + 1; check_x < 4; check_x++)
                {
                    var checkGridPos = new Vector2(check_x, y);
                    var checkBc = CheckBlockController(checkGridPos);
                    if (checkBc == null ||
                        checkBc.isMerge)
                    {
                        count++;
                    }
                    else if (!isNear)
                    {
                        isNear = true;
                        if (bc.number == checkBc.number)
                        {
                            bc.isMerge = true;
                            blockControllerList.Remove(bc);
                            checkBc.ChangeNextBlockNumber();
                            count++;
                            dataHolder.score += 2;
                            scoreText.text = dataHolder.score.ToString();
                        }
                    }

                    isFieldActive[check_x, y] = true;
                }

                if (count > 0)
                {
                    bc.transformRight(count);
                    isMoveBlock = true;
                }
                else
                {
                    isFieldActive[x, y] = true;
                }
            }
        }

        if (!isMoveBlock)
        {
            return;
        }

        CreateNewBlock();
    }

    //左に移動させるメソッド
    private void MoveLeft()
    {
        RefleshFieldActiveList();
        bool isMoveBlock = false;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                var gridPos = new Vector2(x, y);
                var bc = CheckBlockController(gridPos);
                if (bc == null)
                {
                    continue;
                }

                // 移動先検索
                int count = 0;
                bool isNear = false;
                for (var check_x = x - 1; check_x >= 0; check_x--)
                {
                    var checkGridPos = new Vector2(check_x, y);
                    var checkBc = CheckBlockController(checkGridPos);
                    if (checkBc == null ||
                        checkBc.isMerge)
                    {
                        count++;
                    }
                    else if (!isNear)
                    {
                        isNear = true;
                        if (bc.number == checkBc.number)
                        {
                            bc.isMerge = true;
                            blockControllerList.Remove(bc);
                            checkBc.ChangeNextBlockNumber();
                            count++;
                            dataHolder.score += 2;
                            scoreText.text = dataHolder.score.ToString();
                        }
                    }

                    isFieldActive[check_x, y] = true;
                }

                if (count > 0)
                {
                    bc.transformLeft(count);
                    isMoveBlock = true;
                }
                else
                {
                    isFieldActive[x, y] = true;
                }
            }
        }

        if (!isMoveBlock)
        {
            return;
        }

        CreateNewBlock();
    }

    //下に移動させるメソッド
    private void MoveDown()
    {
        RefleshFieldActiveList();
        bool isMoveBlock = false;
        for (var y = 3; y >= 0; y--)
        {
            for (var x = 0; x < 4; x++)
            {
                var gridPos = new Vector2(x, y);
                var bc = CheckBlockController(gridPos);
                if (bc == null)
                {
                    continue;
                }

                // 移動先検索
                int count = 0;
                bool isNear = false;
                for (var check_ｙ = y + 1; check_ｙ < 4; check_ｙ++)
                {
                    var checkGridPos = new Vector2(x, check_ｙ);
                    var checkBc = CheckBlockController(checkGridPos);
                    if (checkBc == null ||
                        checkBc.isMerge)
                    {
                        count++;
                    }
                    else if (!isNear)
                    {
                        isNear = true;
                        if (bc.number == checkBc.number)
                        {
                            bc.isMerge = true;
                            blockControllerList.Remove(bc);
                            checkBc.ChangeNextBlockNumber();
                            count++;
                            dataHolder.score += 2;
                            scoreText.text = dataHolder.score.ToString();
                        }
                    }

                    isFieldActive[x, check_ｙ] = true;
                }

                if (count > 0)
                {
                    bc.transformDown(count);
                    isMoveBlock = true;
                }
                else
                {
                    isFieldActive[x, y] = true;
                }
            }
        }

        if (!isMoveBlock)
        {
            return;
        }

        CreateNewBlock();
    }

    //上に移動させるメソッド
    private void MoveUp()
    {
        RefleshFieldActiveList();
        bool isMoveBlock = false;
        for (var y = 0; y < 4; y++)
        {
            for (var x = 0; x < 4; x++)
            {
                var gridPos = new Vector2(x, y);
                var bc = CheckBlockController(gridPos);
                if (bc == null)
                {
                    continue;
                }

                // 移動先検索
                int count = 0;
                bool isNear = false;
                for (var check_ｙ = y - 1; check_ｙ >= 0; check_ｙ--)
                {
                    var checkGridPos = new Vector2(x, check_ｙ);
                    var checkBc = CheckBlockController(checkGridPos);
                    if (checkBc == null ||
                        checkBc.isMerge)
                    {
                        count++;
                    }
                    else if (!isNear)
                    {
                        isNear = true;
                        if (bc.number == checkBc.number)
                        {
                            bc.isMerge = true;
                            blockControllerList.Remove(bc);
                            checkBc.ChangeNextBlockNumber();
                            count++;
                            dataHolder.score += 2;
                            scoreText.text = dataHolder.score.ToString();
                        }
                    }

                    isFieldActive[x, check_ｙ] = true;
                }

                if (count > 0)
                {
                    bc.transformUp(count);
                    isMoveBlock = true;
                }
                else
                {
                    isFieldActive[x, y] = true;
                }
            }
        }

        if (!isMoveBlock)
        {
            return;
        }

        CreateNewBlock();
    }

    /// <summary>
    /// ブロックコントローラー検索
    /// </summary>
    private BlockController CheckBlockController(Vector2 gridPos)
    {
        BlockController result = null;

        foreach (var blockController in blockControllerList)
        {
            if (blockController.gridPosition == gridPos)
            {
                result = blockController;
                break;
            }
        }

        return result;
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

    private void CheckAndMergeBlocks()
    {
        Debug.Log("OK!");

        for (int i = 0; i < blockControllerList.Count; i++)
        {
            for (int j = i + 1; j < blockControllerList.Count; j++)
            {
                BlockController blockA = blockControllerList[i];
                BlockController blockB = blockControllerList[j];

                //同じ位置にあり、おなじ数字なら合体
                if (blockA.gridPosition == blockB.gridPosition && blockA.number == blockB.number)
                {
                    blockA.MergeBlock(blockB);
                    blockControllerList.Remove(blockB);
                    Destroy(blockB.gameObject);
                    isSE = true;
                }
            }
        }
    }
    
    public void SceneSwitching()
    {
        SceneManager.LoadScene("ClearScene");
    }

    public int GetScore()
    {
        return dataHolder.score;
    }

    public float ReturnTime()
    {
        dataHolder.time = 0; 
        
        return dataHolder.time;
    }
}
