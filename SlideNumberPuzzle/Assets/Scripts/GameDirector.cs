using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム本編シーン全体を管理するクラス
/// </summary>
public class GameDirector : MonoBehaviour
{
    [Header("生成するブロックのプレハブ")]
    [SerializeField] private GameObject SquareNo2 = null;

    [Header("スコアのテキスト")]
    [SerializeField] private TextMeshProUGUI scoreText = null;

    [Header("タイムのテキスト")]
    [SerializeField] private TextMeshProUGUI timeText = null;

    [Header("BloockController型リスト\n各ブロックの位置を精査ために用意")]
    public List<BlockController> blockControllerList = new List<BlockController>();

    [Header("ゲームディレクター")]
    public static GameDirector gameDirector = null;

    [Header("生成位置検出用変数")]
    public int xCoordinate = 0;
    public int yCoordinate = 0;

    // DataHolderオブジェクトの情報を取得するための変数
    private DataHolder dataHolder;

    //スタート時の2つのブロックが同じマスに生成されるのを
    //防ぐために位置を保持する変数
    private int keepX = 0;
    private int keepY = 0;

    //2つのブロックを生成したかを判別するためのカウント用変数
    private int count = 0;

    //フリック入力の時間制限用変数
    private float actionTime = 0;

    //マウスの移動方向を検出するための変数
    private Vector2 startPos = Vector2.zero;

    //移動方向決定時に移動したい方向にブロックがあるかどうかを判定するための2次元配列
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
        // DataHolderオブジェクトを取得
        dataHolder = GameObject.Find("DataHolder").GetComponent<DataHolder>();

        //フレームレートを60に設定
        Application.targetFrameRate = 60;

        //DataHolderのtimeとscoreを初期化
        dataHolder.time = 0;
        dataHolder.score = 0;

        //開始時のブロック二つを生成
        for (; ; )
        {
            //ランダムでx,yの座標を決定
            xCoordinate = Random.Range(0, 8);
            yCoordinate = Random.Range(0, 8);

            //生成用の変数
            int vertical = yCoordinate * -1;
            int horizontal = xCoordinate;

            int gridX = CompareDirection(xCoordinate);
            int gridY = CompareDirection(yCoordinate);

            //ブロックが重ならないようにする処理
            if (keepX != horizontal && keepY != vertical && horizontal % 2 != 0 && vertical % 2 != 0)
            {
                //ブロックを生成
                GameObject square = Instantiate(SquareNo2);
                //生成位置を設定
                square.transform.position = new Vector3(horizontal, vertical, 0);

                //BlockControllerのgridPositionを設定
                BlockController bc = square.GetComponent<BlockController>();
                //bc.gridPosition = new Vector2(Mathf.Abs(Mathf.Floor(xCoordinate / 2.0f)), 
                //                                        Mathf.Floor(yCoordinate / 2.0f));
                bc.gridPosition = new Vector2Int(gridX, gridY);

                //リストに追加
                blockControllerList.Add(bc);

                //生成位置を保持
                keepX = horizontal;
                keepY = vertical;

                //カウントを増やす
                count++;
            }

            //2つ生成したらループを抜ける
            if (count == 2)
            {
                break;
            }
        }
    }

    private void Update()
    {
        // 時間を計測
        dataHolder.time += Time.deltaTime;

        //マウスの連続入力を防ぐために0.5秒に一回しか動けないようにする
        actionTime += Time.deltaTime;

        //タイムを表示
        timeText.text = dataHolder.time.ToString("F1") + "[s]";

        //マウスの入力を検知
        if (Input.GetMouseButtonDown(0))
        {
            //クリックした瞬間のマウスの位置を保存
            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //クリックを離した瞬間のマウスの位置を保存
            Vector2 endPos = Input.mousePosition;

            //クリックした瞬間の座標と離れた瞬間の座標の差を求め
            float xDifference = endPos.x - startPos.x;
            float yDifference = endPos.y - startPos.y;
            //絶対値に変換
            float xAmountOfMovement = Mathf.Abs(xDifference);
            float yAmountOfMovement = Mathf.Abs(yDifference);

            if (actionTime >= 0.5f)
            {
                //マウスの移動した方向にブロックを動かす。
                if (yAmountOfMovement < xAmountOfMovement)
                {
                    if (xDifference < 0)
                    {
                        MoveLeft();
                        actionTime = 0;
                    }
                    else
                    {
                        MoveRight();
                        actionTime = 0;
                    }
                }
                else if (xAmountOfMovement < yAmountOfMovement)
                {
                    if (yDifference < 0)
                    {
                        MoveDown();
                        actionTime = 0;
                    }
                    else
                    {
                        MoveUp();
                        actionTime = 0;
                    }
                }
            }

        }
    }

    /// <summary>
    /// 移動先にブロックがあるかどうかを判定するための2次元配列を初期化するメソッド
    /// </summary>
    private void RefleshFieldActiveList()
    {
        //i、jではなくx、yに変更しわかりやすくした
        for (var x = 0; x < 4; x++)
        {
            for (var y = 0; y < 4; y++)
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
        // Vector2型リストを生成
        var emptyPositionList = new List<Vector2Int>();

        //isFieldActiveぼ各番地を見て
        //falseの番地をVector2型リストemptyPositionListに登録
        for (var x = 0; x < 4; x++)
        {
            for (var y = 0; y < 4; y++)
            {
                if (!isFieldActive[x, y])
                {
                    emptyPositionList.Add(new Vector2Int(x, y));
                }
            }
        }

        //emptyPositionListの要素が0でないなら
        if (emptyPositionList.Count != 0)
        {
            // ランダムな位置を決定
            var random = Random.Range(0, emptyPositionList.Count);
            // random変数で決定したランダムな位置のVector2型変数を保存
            var createPos = emptyPositionList[random];

            //新しくブロックを生成
            GameObject square = Instantiate(SquareNo2);
            square.transform.position = new Vector3(createPos.x * 2.0f + 1.0f, -createPos.y * 2.0f - 1.0f, 0); ;

            //BlockController型リストに生成したブロックのBloockControllerを登録
            BlockController bc = square.GetComponent<BlockController>();
            bc.gridPosition = createPos;
            blockControllerList.Add(bc);
        }
    }

    /// <summary>
    /// 右に移動させるメソッド
    /// </summary>
    private void MoveRight()
    {
        RefleshFieldActiveList();

        // 移動したかどうかを判定する変数
        bool isMoveBlock = false;

        for (int x = 3; x >= 0; x--)
        {
            for (int y = 0; y < 4; y++)
            {
                // ブロックコントローラー取得
                var gridPos = new Vector2(x, y);
                var bc = CheckBlockController(gridPos);

                // ブロックがなければ次へ
                if (bc == null)
                {
                    continue;
                }

                // 移動先検索
                int count = 0;

                // 一番近いブロックと合体したかどうかを判定する変数
                bool isNear = false;

                for (var check_x = x + 1; check_x < 4; check_x++)
                {
                    // チェックする位置のブロックコントローラー取得
                    var checkGridPos = new Vector2(check_x, y);
                    var checkBc = CheckBlockController(checkGridPos);

                    // ブロックがなければ、または合体済みなら移動量を増やす
                    if (checkBc == null ||
                        checkBc.isMerge)
                    {
                        count++;
                    }
                    // 一番近いブロックと合体処理
                    else if (!isNear)
                    {
                        isNear = true;

                        // 数字が同じなら合体
                        if (bc.number == checkBc.number)
                        {
                            // 合体済みにする
                            bc.isMerge = true;

                            // リストから削除
                            blockControllerList.Remove(bc);

                            // ブロックの数字を変更
                            checkBc.ChangeNextBlockNumber();
                            count++;

                            // スコア加算
                            dataHolder.score += 2;
                            // スコア表示更新
                            scoreText.text = dataHolder.score.ToString();
                        }
                    }

                    //移動先の位置に対応するisFieldActiveをtrueにする
                    isFieldActive[check_x, y] = true;
                }

                // ブロックを移動させる
                if (count > 0)
                {
                    string direction = "Horizontal";

                    // ブロックを移動
                    bc.BlockMoveDirection(count, direction);

                    // 移動したことを判定
                    isMoveBlock = true;
                }
                else
                {
                    // 移動先がなかった場合、現在位置に対応するisFieldActiveをtrueにする
                    isFieldActive[x, y] = true;
                }
            }
        }

        // ブロックが移動していなければ処理を抜ける
        if (!isMoveBlock)
        {
            return;
        }

        // 新しいブロックを生成
        CreateNewBlock();
    }

    /// <summary>
    /// 左に移動させるメソッド
    /// </summary>
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
                    string direction = "Horizontal";
                    bc.BlockMoveDirection(-count, direction);
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
                    string direction = "Vertical";
                    bc.BlockMoveDirection(-count, direction);
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
                    string direction = "Vertical";
                    bc.BlockMoveDirection(count, direction);
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
            // リスト内のブロックに同じ座標のブロックがいれば値を返す
            if (Mathf.Approximately(blockController.gridPosition.x, gridPos.x) &&
                Mathf.Approximately(blockController.gridPosition.y, gridPos.y))//blockController.gridPosition == gridPos
            {
                result = blockController;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// ランダムで決定した数値を基に位置を修正するメソッド
    /// </summary>
    public int CompareDirection(int Coordinate)
    {
        //if (Coordinate == 1)
        //{
        //    Coordinate = 0;
        //}
        //else if (Coordinate == 3)
        //{
        //    Coordinate = 1;
        //}
        //else if (Coordinate == 5)
        //{
        //    Coordinate = 2;
        //}
        //else if (Coordinate == 7)
        //{
        //    Coordinate = 3;
        //}

        //return Coordinate;

        return Mathf.Clamp(Coordinate / 2, 0, 3);
    }


    /// <summary>
    /// クリア判定が出たときにシーン移動をする
    /// </summary>
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
