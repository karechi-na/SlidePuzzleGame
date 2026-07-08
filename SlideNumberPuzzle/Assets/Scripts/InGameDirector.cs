using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameDirector : MonoBehaviour
{
    [Header("Square")]
    [SerializeField] private GameObject SquareNo2;

    [SerializeField] private TextMeshProUGUI scoretext;
    [SerializeField] private TextMeshProUGUI timeText;

    private float actionTime = 0.0f;

    private Vector2 startPos = Vector2.zero;

    public List<BlockController> blockControllerList = new List<BlockController>();

    private bool[,] isFieldActive = new bool[4, 4];

    private void Start()
    {
        Application.targetFrameRate = 60;

        DataHolder.Instance.DataInitialization();

        SpawnInitialBlocks();
    }

    private void Update()
    {
        UpdateTime();
        HandleInput();
    }

    //----------------------------------
    // 初期ブロックの生成
    //----------------------------------

    private void SpawnInitialBlocks()
    {
        for (int i = 0; i < 2; i++)
        {
            CreateNewBlock();
        }
    }

    //----------------------------------
    // 入力
    //----------------------------------

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
            startPos = Input.mousePosition;
        if (Input.GetMouseButtonUp(0))
            ProcessSwipe(Input.mousePosition);
    }

    private void ProcessSwipe(Vector2 endPos)
    {
        if (actionTime < 0.3f) return;

        float x = Mathf.Abs(endPos.x - startPos.x);
        float y = Mathf.Abs(endPos.y - startPos.y);

        if (x > y)
        {
            if (endPos.x > startPos.x)
                Move(Vector2Int.right);
            else
                Move(Vector2Int.left);
        }
        else
        {
            if (endPos.y > startPos.y)
                Move(Vector2Int.up);
            else
                Move(Vector2Int.down);
        }

        actionTime = 0.0f;
    }

    //----------------------------------
    // 移動処理（統合）
    //----------------------------------

    private void Move(Vector2Int dir)
    {
        foreach(var b in blockControllerList)
            b.isMerge = false;

        ResetFieldState();

        bool moved = false;

        List<BlockController> sorted = new List<BlockController>(blockControllerList);

        // 移動方向によって処理順を変える
        sorted.Sort((a, b) =>
        {
            if (dir.x == 1) return b.gridPosition.x.CompareTo(a.gridPosition.x);
            if (dir.x == -1) return a.gridPosition.x.CompareTo(b.gridPosition.x);
            if (dir.y == 1) return b.gridPosition.y.CompareTo(a.gridPosition.y);
            return a.gridPosition.y.CompareTo(b.gridPosition.y);
        });

        foreach (var block in sorted)
        {
            moved |= MoveBlock(block, dir);
        }

        if (moved)
        {
            CreateNewBlock();
        }
    }

    //----------------------------------
    // ブロック1個の移動
    //----------------------------------

    private bool MoveBlock(BlockController bc, Vector2Int dir)
    {
        Vector2Int pos = Vector2Int.RoundToInt(bc.gridPosition);

        int moveCount = 0;

        while (true)
        {
            Vector2Int next = pos + dir;

            if (next.x < 0 || next.x > 3 || next.y < 0 || next.y > 3) break;

            var other = GetBlock(next);

            if (other == null)
            {
                pos = next;
                moveCount++;
            }
            else
            {
                if (other.number == bc.number && !other.isMerge)
                {
                    other.ChangeNextBlockNumber();
                    bc.isMerge = true;

                    blockControllerList.Remove(bc);

                    moveCount++;
                }
                break;
            }
        }

        if (moveCount == 0) return false;

        bc.gridPosition += dir * moveCount;

        if (dir == Vector2Int.right)
            bc.transformRight(moveCount);
        else if (dir == Vector2Int.left)
            bc.transformLeft(moveCount);
        else if (dir == Vector2Int.up)
            bc.transformUp(moveCount);
        else
            bc.transformDown(moveCount);

        return true;
    }

    //----------------------------------
    // ブロックの取得
    //----------------------------------

    private BlockController GetBlock(Vector2Int gridPos)
    {
        foreach (var b in blockControllerList)
        {
            if (Vector2Int.RoundToInt(b.gridPosition) == gridPos)
                return b;
        }

        return null;
    }

    //----------------------------------
    // 新規ブロックの生成
    //----------------------------------

    private void CreateNewBlock()
    {
        List<Vector2Int> empty = new List<Vector2Int>();

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (GetBlock(new Vector2Int(x, y)) == null)
                    empty.Add(new Vector2Int(x, y));
            }
        }

        if (empty.Count == 0) return;

        Vector2Int grid = empty[Random.Range(0, empty.Count)];

        SpawnBlock(grid);
    }

    private void SpawnBlock(Vector2Int grid)
    {
        Vector3 pos = new Vector3(
            grid.x * 2 + 1,
            -grid.y * 2 - 1,
            0.0f
        );

        GameObject obj = Instantiate(SquareNo2, pos, Quaternion.identity);

        BlockController bc = obj.GetComponent<BlockController>();

        bc.gridPosition = grid;

        blockControllerList.Add(bc);
    }

    //-----------------------------------

    private void ResetFieldState()
    {
        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
                isFieldActive[x, y] = false;
    }

    //----------------------------------

    private void UpdateTime()
    {
        DataHolder.Instance.time += Time.deltaTime;
        actionTime += Time.deltaTime;

        InGameUIManager.Instance.UpdateTime();
    }
}
