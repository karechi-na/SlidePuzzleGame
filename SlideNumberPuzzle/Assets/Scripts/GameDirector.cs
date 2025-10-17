using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// マウスを動かす方向に向かってブロックを動かしたりブロックを管理する処理
/// </summary>
public class GameDirector : MonoBehaviour
{
    [Header("ボックスのPrefabオブジェクト")]
    [SerializeField] private GameObject SquareNo2;

    [Header("スコアテキスト")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("タイムテキスト")]
    [SerializeField] private TextMeshProUGUI timeText;

    //keepX,keepYは開始時に生成するブロックの位置を保持する変数
    private int keepX = 0;
    private int keepY = 0;

    //開始時に生成するブロックの数を数える変数
    private int count = 0;
    private int moveBlockCount = 0;

    //ブロックを動かした後のクールタイム
    private float actionTime = 0;

    //xCoordinate,yCoordinateは開始時に生成するブロックの位置を決める変数
    public int xCoordinate = 0;
    public int yCoordinate = 0;

    //SEを鳴らすかどうかのフラグ
    public bool isSE = false;

    //マウスを左クリックしたときのposition
    private Vector2 startPos;

    //ブロックが表示されているか管理しているList
    public List<BlockController> blockControllerList = new List<BlockController>();
    public static GameDirector gameDirector;
    private DataHolder dataHolder;

    //dataHolderのタイムとスコアの初期値
    private float initialValueTime = 0.0f;
    private int initialValueScore = 0;

    //マウスの初期位置
    private float InitialMousePositionX = 0.0f;
    private float InitialMousePositionY = 0.0f;

    //パズルの盤面の配置
    //4*4列の盤面になっている
    private bool[,] isFieldActive = new bool[,]
    {
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
    };

    /// <summary>
    /// タイムとスコアの初期化と開始時にブロックを二つ生成する処理
    /// </summary>
    void Start()
    {
        dataHolder = GameObject.Find("DataHolder").GetComponent<DataHolder>();
        Application.targetFrameRate = 60;

        dataHolder.time = initialValueTime;
        dataHolder.score = initialValueScore;

        //開始時のブロック二つを生成
        for (; ; )
        {
            //ランダムでブロックの位置を決める
            xCoordinate = Random.Range(0, 8);
            yCoordinate = Random.Range(0, 8);

            //生成用の変数
            int vertical = yCoordinate * -1;
            int horizontal = xCoordinate;

            //xCoordinate,yCoordinateの数値が一定の数なら数値を変更する処理
            CompareDirectionX(xCoordinate);
            CompareDirectionY(yCoordinate);

            //ランダム関数で決めた数値が前に生成したブロックの座標の数値と被っていない場合
            if (keepX != horizontal && keepY != vertical && horizontal % 2 != 0 && vertical % 2 != 0)
            {
                //ブロックを生成
                GameObject square = Instantiate(SquareNo2);
                square.transform.position = new Vector3(horizontal, vertical, 0);

                //BlockControllerを取得
                BlockController blockController = square.GetComponent<BlockController>();

                //取得したBlockControllerにブロックの座標を設定してblockControllerListに追加
                blockController.gridPosition = new Vector2(Mathf.Abs(Mathf.Floor(xCoordinate / 2.0f)), Mathf.Floor(yCoordinate / 2.0f));
                blockControllerList.Add(blockController);

                keepX = horizontal;
                keepY = vertical;
                count++;
            }

            //生成したブロックの数が2つになったらループを終了
            if (count == 2)
            {
                break;
            }
        }
    }

    /// <summary>
    /// マウスを使ってブロックを動かす処理
    /// </summary>
    private void Update()
    {
        float xPosi = InitialMousePositionX;
        float yPosi = InitialMousePositionY;

        //経過時間を計測するため時間を加算し続ける
        dataHolder.time += Time.deltaTime;
        actionTime += Time.deltaTime;


        timeText.text = dataHolder.time.ToString("F1") + "[s]";

        //マウスの左クリックを押したときの位置を取得
        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //マウスの左クリックを離したときの位置を取得
            Vector2 endPos = Input.mousePosition;

            //左クリックを離したときの位置-押したときの位置の計算をして、どの位置に移動したか計算
            xPosi = Mathf.Abs(endPos.x - this.startPos.x);
            yPosi = Mathf.Abs(endPos.y - this.startPos.y);

            //マウスを動かした時間が0.5秒以上経っていた場合
            if (actionTime >= 0.5f)
            {
                //マウスの移動した方向にブロックを動かす。
                if (yPosi < xPosi && endPos.x - this.startPos.x < 0)
                {
                    MoveLeft();

                    //マウスを動かした後クールタイムを発生させるため0に戻す
                    actionTime = 0;
                }
                else if (yPosi < xPosi && endPos.x - this.startPos.x > 0)
                {
                    MoveRight();

                    //マウスを動かした後クールタイムを発生させるため0に戻す
                    actionTime = 0;
                }
                else if (xPosi < yPosi && endPos.y - this.startPos.y < 0)
                {
                    MoveDown();

                    //マウスを動かした後クールタイムを発生させるため0に戻す
                    actionTime = 0;
                }
                else if (xPosi < yPosi && endPos.y - this.startPos.y > 0)
                {
                    MoveUp();

                    //マウスを動かした後クールタイムを発生させるため0に戻す
                    actionTime = 0;
                }
            }

        }
    }

    /// <summary>
    /// ブロックを移動するときに盤面の配置をリフレッシュする処理
    /// </summary>
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

    /// <summary>
    /// 盤面の中で新しくブロックを生成するときランダムな座標で生成する処理
    /// </summary>
    private void CreateNewBlock()
    {
        var emptyPositionList = new List<Vector2>();

        //盤面のx座標とy座標をループで回す
        for (var x = 0; x < 4; x++)
        {
            for (var y = 0; y < 4; y++)
            {
                if (!isFieldActive[x, y])
                {
                    //盤面の座標を保持してリストに追加する
                    emptyPositionList.Add(new Vector2(x, y));
                }
            }
        }

        //emptyPositionListに中身がある場合
        if (emptyPositionList.Count != 0)
        {
            //emptyPositionListのxyの座標のうちランダムで一つ選ぶ
            var random = Random.Range(0, emptyPositionList.Count);
            var createPos = emptyPositionList[random];

            //emptyPositionListで選んだ座標にブロックを生成する
            GameObject square = Instantiate(SquareNo2);
            square.transform.position = new Vector3(createPos.x * 2.0f + 1.0f, -createPos.y * 2.0f - 1.0f, 0); ;

            //BlockControllerを取得して生成したときの座標をblockControllerListに追加する
            BlockController blockController = square.GetComponent<BlockController>();
            blockController.gridPosition = createPos;
            blockControllerList.Add(blockController);
        }
    }

    //右に移動させるメソッド
    private void MoveRight()
    {
        RefleshFieldActiveList();
        bool isMoveBlock = false;

        //xを右から左へループで回す
        //x--を使っているからxを減らしながらループする
        for (int x = 3; x >= 0; x--)
        {
            for (int y = 0; y < 4; y++)
            {
                var gridPos = new Vector2(x, y);
                var blockController = CheckBlockController(gridPos);
                if (blockController == null)
                {
                    //ブロックが存在しない場合は次のループへ
                    continue;
                }

                // 移動先検索
                moveBlockCount = 0;
                bool isNear = false;
                for (var check_x = x + 1; check_x < 4; check_x++)
                {
                    //ブロックがないマスの座標を取得
                    var checkGridPos = new Vector2(check_x, y);

                    //ブロックコントローラーを取得
                    var checkBc = CheckBlockController(checkGridPos);

                    //マスにブロックが存在しないか、他のブロックが合体済みの場合
                    if (checkBc == null ||
                        checkBc.isMerge)
                    {
                        //ブロックが動ける数を増やす
                        moveBlockCount++;
                    }
                    else if (!isNear)
                    {
                        isNear = true;
                        //ブロック同士の数値が同じ場合
                        if (blockController.number == checkBc.number)
                        {
                            blockController.isMerge = true;

                            //合体したブロックをblockControllerListから削除する
                            blockControllerList.Remove(blockController);

                            //ブロックを合体して画像を変更して数値を増やす
                            checkBc.ChangeNextBlockNumber();

                            //ブロックが動ける数を増やす
                            moveBlockCount++;

                            // スコアを加算する
                            dataHolder.score += 2;

                            // スコアテキストを更新する
                            scoreText.text = dataHolder.score.ToString();
                        }
                    }
                    //盤面の配置を更新する
                    isFieldActive[check_x, y] = true;
                }

                //ブロックが動ける数が0より大きい場合
                if (moveBlockCount > 0)
                {
                    //ブロックを移動させるメソッドを呼び出す
                    blockController.transformRight(moveBlockCount);
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

        //xを左から右へループで回す
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                var gridPos = new Vector2(x, y);
                var bc = CheckBlockController(gridPos);
                if (bc == null)
                {
                    //ブロックが存在しない場合は次のループへ
                    continue;
                }

                // 移動先検索
                moveBlockCount = 0;
                bool isNear = false;
                for (var check_x = x - 1; check_x >= 0; check_x--)
                {
                    var checkGridPos = new Vector2(check_x, y);
                    var checkBc = CheckBlockController(checkGridPos);

                    //マスにブロックが存在しないか、他のブロックが合体済みの場合
                    if (checkBc == null ||
                        checkBc.isMerge)
                    {
                        //ブロックが動ける数を増やす
                        moveBlockCount++;
                    }
                    else if (!isNear)
                    {
                        isNear = true;

                        //ブロック同士の数値が同じ場合
                        if (bc.number == checkBc.number)
                        {
                            bc.isMerge = true;
                            blockControllerList.Remove(bc);
                            checkBc.ChangeNextBlockNumber();
                            moveBlockCount++;
                            dataHolder.score += 2;
                            scoreText.text = dataHolder.score.ToString();
                        }
                    }

                    isFieldActive[check_x, y] = true;
                }

                if (moveBlockCount > 0)
                {
                    bc.transformLeft(moveBlockCount);
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

        //ブロックの移動が終わった後新しいブロックを生成する
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
                moveBlockCount = 0;
                bool isNear = false;
                for (var check_ｙ = y + 1; check_ｙ < 4; check_ｙ++)
                {
                    var checkGridPos = new Vector2(x, check_ｙ);
                    var checkBc = CheckBlockController(checkGridPos);
                    if (checkBc == null ||
                        checkBc.isMerge)
                    {
                        moveBlockCount++;
                    }
                    else if (!isNear)
                    {
                        isNear = true;
                        if (bc.number == checkBc.number)
                        {
                            bc.isMerge = true;
                            blockControllerList.Remove(bc);
                            checkBc.ChangeNextBlockNumber();
                            moveBlockCount++;
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
                moveBlockCount = 0;
                bool isNear = false;
                for (var check_ｙ = y - 1; check_ｙ >= 0; check_ｙ--)
                {
                    var checkGridPos = new Vector2(x, check_ｙ);
                    var checkBc = CheckBlockController(checkGridPos);
                    if (checkBc == null ||
                        checkBc.isMerge)
                    {
                        moveBlockCount++;
                    }
                    else if (!isNear)
                    {
                        isNear = true;
                        if (bc.number == checkBc.number)
                        {
                            bc.isMerge = true;
                            blockControllerList.Remove(bc);
                            checkBc.ChangeNextBlockNumber();
                            moveBlockCount++;
                            dataHolder.score += 2;
                            scoreText.text = dataHolder.score.ToString();
                        }
                    }

                    isFieldActive[x, check_ｙ] = true;
                }

                if (moveBlockCount > 0)
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

        //blockControllerListの中からgridPosと同じ座標を持つBlockControllerを探す
        foreach (var blockController in blockControllerList)
        {
            //
            if (blockController.gridPosition == gridPos)
            {
                result = blockController;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// xCoordinateの数値が一定の数なら数値を変更する処理
    /// </summary>
    /// <param name="xCoordinate"></param>
    /// <returns></returns>
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

    /// <summary>
    /// yCoordinateの数値が一定の数なら数値を変更する処理
    /// </summary>
    /// <param name="yCoordinate"></param>
    /// <returns></returns>
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
