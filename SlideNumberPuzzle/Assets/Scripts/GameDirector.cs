//==========================================================================================================
// 製作者 : スズキ チヒロ
//==========================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [Header("ゲーム開始時に生成するブロックのプレハブ")]
    [SerializeField] private GameObject SquareNo2;
    // スコアと時間のテキスト
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

    private void Start()
    {
        SetFrameRate();

        InitializeData();

        SpawnInitialBlocks();
        ////開始時のブロック二つを生成
        //for (; ; )
        //{
        //    //ランダムに座標を生成
        //    xCoordinate = Random.Range(0, 8);
        //    yCoordinate = Random.Range(0, 8);

        //    //生成用の変数
        //    int vertical = yCoordinate * -1;
        //    int horizontal = xCoordinate;

        //    CompareDirectionX(xCoordinate);
        //    CompareDirectionY(yCoordinate);

        //    if (keepX != horizontal && keepY != vertical && horizontal % 2 != 0 && vertical % 2 != 0)
        //    {
        //        GameObject square = Instantiate(SquareNo2);
        //        square.transform.position = new Vector3(horizontal, vertical, 0);
        //        BlockController bc = square.GetComponent<BlockController>();
        //        bc.gridPosition = new Vector2(Mathf.Abs(Mathf.Floor(xCoordinate / 2.0f)), Mathf.Floor(yCoordinate / 2.0f));
        //        blockControllerList.Add(bc);

        //        keepX = horizontal;
        //        keepY = vertical;
        //        count++;
        //    }
        //    if (count == 2)
        //    {
        //        break;
        //    }
        //}
    }
    private void Update()
    {
        float xPosi = 0;
        float yPosi = 0;

        UpdateTime();

        HandleInput();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    this.startPos = Input.mousePosition;
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    Vector2 endPos = Input.mousePosition;
        //    xPosi = Mathf.Abs(endPos.x - this.startPos.x);
        //    yPosi = Mathf.Abs(endPos.y - this.startPos.y);
        //    if (actionTime >= 0.5f)
        //    {
        //        //マウスの移動した方向にブロックを動かす。
        //        if (yPosi < xPosi && endPos.x - this.startPos.x < 0)
        //        {
        //            MoveLeft();
        //            actionTime = 0;
        //        }
        //        else if (yPosi < xPosi && endPos.x - this.startPos.x > 0)
        //        {
        //            MoveRight();
        //            actionTime = 0;
        //        }
        //        else if (xPosi < yPosi && endPos.y - this.startPos.y < 0)
        //        {
        //            MoveDown();
        //            actionTime = 0;
        //        }
        //        else if (xPosi < yPosi && endPos.y - this.startPos.y > 0)
        //        {
        //            MoveUp();
        //            actionTime = 0;
        //        }
        //    }

        //}
        ////if ()
        ////{
        ////    SceneManager.LoadScene("ClearScene");
        ////}
    }

    #region 初期化関連
    /// <summary>
    /// フレームレートの設定メソッド
    /// </summary>
    private void SetFrameRate()
    {
        // フレームレートの設定
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// DataHolderの初期化メソッド
    /// </summary>
    private void InitializeData()
    {
        // DataHolderの取得と初期化
        DataHolder.Instance.DataInitialization();
    }

    /// <summary>
    /// 最初の2つのブロックを生成するメソッド
    /// </summary>
    private void SpawnInitialBlocks()
    {
        int spwnCount = 0;

        while (spwnCount < 2)
        {
            Vector2Int randomPos = GetRandomSpawnPosition();

            if (CanSpawnBlock(randomPos))
            {
                CreateBlock(randomPos);
                spwnCount++;
            }
        }
    }

    /// <summary>
    /// ランダムな生成位置を取得するメソッド
    /// </summary>
    private Vector2Int GetRandomSpawnPosition()
    {
        int x = Random.Range(0, 8);
        int y = Random.Range(0, 8);

        return new Vector2Int(x, y);
    }

    /// <summary>
    /// ランダムに生成された位置にブロックを生成できるかを判定するメソッド
    /// </summary>
    private bool CanSpawnBlock(Vector2Int pos)
    {
        int horizontal = pos.x;
        int vertical = pos.y * -1;

        if (keepX != horizontal &&
            keepY != vertical &&
            horizontal % 2 != 0 &&
            vertical % 2 != 0)
        {
            keepX = horizontal;
            keepY = vertical;
            return true;
        }

        return false;
    }

    /// <summary>
    /// ランダムに生成された位置にブロックを生成するメソッド
    /// </summary>
    private void CreateBlock(Vector2Int pos)
    {
        int horizontal = pos.x;
        int vertical = pos.y * -1;

        GameObject square = Instantiate(SquareNo2);
        square.transform.position = new Vector3(horizontal, vertical, 0);

        BlockController bc = square.GetComponent<BlockController>();
        bc.gridPosition = new Vector2(
            horizontal / 2.0f,
            vertical / 2.0f
            );

        blockControllerList.Add(bc);
    }
    #endregion


    /// <summary>
    /// 時間更新処理メソッド
    /// </summary>
    private void UpdateTime()
    {
        DataHolder.Instance.time += Time.deltaTime;
        actionTime += Time.deltaTime;

        InGameUIManager.Instance.UpdateTime();
    }

    /// <summary>
    /// マウスの入力を処理するメソッド
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            ProcessSwipe(Input.mousePosition);
        }
    }

    /// <summary>
    /// マウスのスワイプ方向によってブロックを移動させるメソッド
    /// </summary>
    private void ProcessSwipe(Vector2 endPos)
    {
        if (actionTime < 0.5f) return;

        float x = Mathf.Abs(endPos.x - startPos.x);
        float y = Mathf.Abs(endPos.y - startPos.y);

        if (y < x)
        {
            if (endPos.x < startPos.x)
                MoveLeft();
            else
                MoveRight();
        }
        else
        {
            if (endPos.y < startPos.y)
                MoveDown();
            else
                MoveUp();
        }
    }

    /// <summary>
    /// fieldの状態をリセットするメソッド
    /// </summary>
    private void ResetFieldState()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                isFieldActive[x, y] = false;
            }
        }
    }

    /// <summary>
    /// 新しいブロックを生成するメソッド
    /// </summary>
    private void CreateNewBlock()
    {
        List<Vector2> emptyCells = GetEmptyCells();

        if (emptyCells.Count == 0) return;

        Vector2 pos = emptyCells[Random.Range(0, emptyCells.Count)];
        SpawnBlockAt(pos);
    }

    /// <summary>
    /// 空いているセルのリストを取得するメソッド
    /// </summary>
    private List<Vector2> GetEmptyCells()
    {
        List<Vector2> list = new List<Vector2>();
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (!isFieldActive[x, y])
                {
                    list.Add(new Vector2(x, y));
                }
            }
        }

        return list;
    }

    /// <summary>
    /// GetEmptyCellsで取得した空いているセルの位置からランダムにブロックを生成するメソッド
    /// </summary>
    private void SpawnBlockAt(Vector2 pos)
    {
        GameObject square = Instantiate(SquareNo2);
        square.transform.position =
            new Vector3(pos.x * 2.0f + 1.0f, -pos.y * 2.0f - 1.0f, 0);

        BlockController bc = square.GetComponent<BlockController>();
        bc.gridPosition = pos;

        blockControllerList.Add(bc);
    }

    /// <summary>
    /// 右に移動させるメソッド
    /// </summary>
    private void MoveRight()
    {
        ResetFieldState();
        bool isMoveBlock = false;
        for (int x = 3; x >= 0; x--)
        {
            for (int y = 0; y < 4; y++)
            {
                var gridPos = new Vector2(x, y);
                var bc = CheckBlockController(gridPos);
                if (bc == null)
                {
                    Debug.Log("Check : " + gridPos);
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
                        Debug.Log("MoveRight1");

                        count++;
                    }
                    else if (!isNear)
                    {
                        isNear = true;
                        if (bc.number == checkBc.number)
                        {
                            Debug.Log("MoveRight2");
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
                    Debug.Log("MoveRight3");
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

    /// <summary>
    /// 左に移動させるメソッド
    /// </summary>
    private void MoveLeft()
    {
        ResetFieldState();
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

    /// <summary>
    /// 下に移動させるメソッド
    /// </summary>
    private void MoveDown()
    {
        ResetFieldState();
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

    /// <summary>
    /// 上に移動させるメソッド
    /// </summary>
    private void MoveUp()
    {
        ResetFieldState();
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
            Debug.Log("CheckBlockController: " + blockController.gridPosition);
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

    //public int GetScore()
    //{
    //    return dataHolder.score;
    //}

    //public float ReturnTime()
    //{
    //    dataHolder.time = 0; 

    //    return dataHolder.time;
    //}

}
