using UnityEngine;
using DG.Tweening;

/// <summary>
/// 主にブロックを動かす処理を行う
/// </summary>
public class BlockController : MonoBehaviour
{
    [Header("イメージの配列(０から順に２～２０４８までいれる)")]
    [SerializeField] private Sprite[] numberSpriteArray = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    public bool isTransform = false;
    private GameDirector gameDirector;
    public Vector2 gridPosition = Vector2.zero;
    public int number { private set; get; } = 0;
    public bool isMerge = false;
    //ゲーム上では2の11乗で2048になる
    private int blockNumber = 0;

    //ブロックが一定の距離を移動させるためにかかる時間
    private float blockMoveTime = 0.0f;

    //ブロックが移動する数値
    private float blockMovePosition = 0.0f;

    void Start()
    {
        //画像を配列に設定してあるものに変更する
        spriteRenderer.sprite = numberSpriteArray[number];

        //ゲーム上では2の11乗で2048になる
        blockNumber = 11;

        //0.5秒かけて移動させる
        blockMoveTime = 0.5f;

        //ブロックが移動する数値
        blockMovePosition = 2.0f;
    }

    public void ChangeNextBlockNumber()
    {
        number++;
        spriteRenderer.sprite = numberSpriteArray[number];

        //ゲームの数を加算していき、2048の数字になったらクリアシーンへシーン移行
        //number == 11は2の11乗（2048）になる
        if (number == blockNumber)
        {
            gameDirector.SceneSwitching();
        }
    }

    //左に移動
    public Tween transformLeft(int moveGridCount)
    {
        //DOTween(DOMove)の機能で滑らかに移動させる
        //0.5秒かけて左（-x方向）に移動する
        return this.transform.DOMove(new Vector3(transform.position.x - (blockMovePosition * moveGridCount), transform.position.y, 0), blockMoveTime)
                        .OnComplete(() =>
                        {
                            //動かした後隣にいるブロックと数字が同じだったら合体させる
                            if (isMerge)
                            {
                                Destroy(gameObject);
                            }
                            //違う場合は移動した分だけ位置を更新する
                            else
                            {
                                gridPosition.x -= moveGridCount;
                            }
                        });
    }

    //右に移動
    public Tween transformRight(int moveGridCount)
    {
        //0.5秒かけて右（x方向）に移動する
        return this.transform.DOMove(new Vector3(transform.position.x + (blockMovePosition * moveGridCount), transform.position.y, 0), blockMoveTime)
                    .OnComplete(() =>
                    {
                        //動かした後隣にいるブロックと数字が同じだったら合体させる
                        if (isMerge)
                        {
                            Destroy(gameObject);
                        }
                        else
                        {
                            gridPosition.x += moveGridCount;
                        }
                    });
    }

    //下に移動
    public Tween transformDown(int moveGridCount)
    {
        //0.5秒かけて下（-y方向）に移動する
        return this.transform.DOMove(new Vector3(transform.position.x, transform.position.y - (blockMovePosition * moveGridCount), 0), blockMoveTime)
            .OnComplete(() =>
            {
                //動かした後隣にいるブロックと数字が同じだったら合体させる
                if (isMerge)
                {
                    Destroy(gameObject);
                }
                else
                {
                    gridPosition.y += moveGridCount;
                }
            });
    }

    //上に移動
    public Tween transformUp(int moveGridCount)
    {
        //0.5秒かけて上（y方向）に移動する
        return this.transform.DOMove(new Vector3(transform.position.x, transform.position.y + (blockMovePosition * moveGridCount), 0), blockMoveTime)
                        .OnComplete(() =>
                        {
                            //動かした後隣にいるブロックと数字が同じだったら合体させる
                            if (isMerge)
                            {
                                Destroy(gameObject);
                            }
                            else
                            {
                                gridPosition.y -= moveGridCount;
                            }
                        });
    }

    /// <summary>
    /// 処理の中でブロックの数を2倍にする
    /// </summary>
    /// <param name="otherBlock"></param>
    public void MergeBlock(BlockController otherBlock)
    {
        // ブロックの数を2倍にしてその数の画像に変更する
        this.number *= 2;
        Debug.Log("Merged block number:" + this.number);
        UpdateSprite();
    }

    /// <summary>
    /// 合体したブロックの画像を更新する
    /// </summary>
    private void UpdateSprite()
    {
        //numberの2乗の数値を計算してその数の画像を表示する
        //配列のnumberは0から始まるので-1する
        int index = (int)Mathf.Log(number, 2) - 1;
        if (index >= 0 && index < numberSpriteArray.Length)
        {
            //ブロックが合体した後画像を変更する
            spriteRenderer.sprite = numberSpriteArray[index];
        }
        else
        {
            //対応する画像がない場合エラーとして表示させる
            Debug.LogError("Sprite index out of bounds: " + index + ", for number: " + number);
        }
    }
}
