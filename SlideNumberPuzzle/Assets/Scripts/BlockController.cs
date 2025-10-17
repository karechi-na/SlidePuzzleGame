using UnityEngine;
using DG.Tweening;

/// <summary>
/// 各ブロックの移動、合体を管理するクラス
/// </summary>
public class BlockController : MonoBehaviour
{
    [Header("イメージの配列(0から順に2～2048までいれる)")]
    [SerializeField] private Sprite[] numberSpriteArray = null;

    [Header("自身のスプライトレンダラーの情報(開始時は2)")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;


    [Header("ブロックの座標を取る")]
    public Vector2Int gridPosition = Vector2Int.zero;

    [Header("合体したかを見る")]
    public bool isMerge = false;

    //ブロックの数字の管理
    //0～11　numberSpriteArrayの番地を管理
    public int number { get; private set; } = 0;

    //ゲームディレクターの情報
    private GameDirector gameDirector = null;

    //クリア条件の数字
    private int numberOfCleerCondition = 11; // 2048


    void Start()
    {
        //開始時の数字2を設定
        spriteRenderer.sprite = numberSpriteArray[number];
    }

    /// <summary>
    /// 同じ数字がぶつかったときに呼ばれる
    /// </summary>
    public void ChangeNextBlockNumber()
    {
        // 次の数字に変更
        number++;
        spriteRenderer.sprite = numberSpriteArray[number];

        //数字が2048になったらクリア
        if (number == numberOfCleerCondition)
        {
            //シーン切り替え
            gameDirector.SceneSwitching();
        }
    }


    /// <summary>
    /// moveGridCountの値分移動
    /// moveDirectionで方向を指定
    /// 値が正なら上または右、負なら下または左
    /// </summary>
    public Tween BlockMoveDirection(int moveGridCount, string moveDirection)
    {
        Vector3 targetPos = transform.position;
        //　水平方向ならX座標、垂直方向ならY座標を変更
        if (moveDirection == "Horizontal")
        {
            targetPos.x += 2.0f * moveGridCount;
        }
        else if(moveDirection == "Vertical")
        {
            targetPos.y += 2.0f * moveGridCount;
        }

        // 移動アニメーション
        return transform.DOMove(
                new Vector3(targetPos.x, targetPos.y, 0.0f), 0.5f)
                .OnComplete(() =>
                {
                    if (isMerge)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        //gridPosition.x += moveGridCount;

                        if (moveDirection == "Horizontal")
                        {
                            gridPosition.x = Mathf.Clamp(gridPosition.x + moveGridCount, 0, 3);
                        }
                        else if (moveDirection == "Vertical")
                        {
                            gridPosition.y = Mathf.Clamp(gridPosition.y + moveGridCount, 0, 3);
                        }
                    }
                }
                );
    }


    ///// <summary>
    ///// 
    ///// </summary>
    //public void MergeBlock(BlockController otherBlock)
    //{
    //    this.number *= 2;
    //    Debug.Log("Merged block number:" + this.number);
    //    UpdateSprite();
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    //private void UpdateSprite()
    //{
    //    int index = (int)Mathf.Log(number, 2) - 1;
    //    if (index >= 0 && index < numberSpriteArray.Length)
    //    {
    //        spriteRenderer.sprite = numberSpriteArray[index];
    //    }
    //    else
    //    {
    //        Debug.LogError("Sprite index out of bounds: " + index + ", for number: " + number);
    //    }
    //}
}
