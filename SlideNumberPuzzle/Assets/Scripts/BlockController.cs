using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    [Header("イメージの配列(0から順に2～2048までいれる)")]
    [SerializeField] private Sprite[] numberSpriteArray = null;

    [Header("")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    [Header("")]
    public bool isTransform = false;

    //
    private GameDirector gameDirector = null;

    [Header("")]
    public Vector2 gridPosition = Vector2.zero;

    [Header("")]
    public int number { get; private set; } = 0;

    [Header("")]
    public bool isMerge = false;

    void Start()
    {
        //
        spriteRenderer.sprite = numberSpriteArray[number];
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangeNextBlockNumber()
    {
        number++;
        spriteRenderer.sprite = numberSpriteArray[number];

        //
        if (number == 11)
        {
            gameDirector.SceneSwitching();
        }
    }

    /// <summary>
    /// moveGridCountの値分左右に移動
    /// 値が正なら右、負なら左
    /// </summary>
    public Tween BlockTransformHorizontal(int moveGridCount)
    {
        return this.transform.DOMove(new Vector3(transform.position.x + (2.0f * moveGridCount),
                                                 transform.position.y,
                                                 0.0f),
                                                 0.5f)
                             .OnComplete(() =>
                             {
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

    /// <summary>
    /// moveGridCountの値分上下に移動
    /// 値が正なら上、負なら下
    /// </summary>
    public Tween BlockTransformVertical(int moveGridCount)
    {
        return this.transform.DOMove(new Vector3(transform.position.x,
                                                 transform.position.y + (2.0f * moveGridCount),
                                                 0.0f),
                                                 0.5f)
                             .OnComplete(() =>
                             {
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

    /// <summary>
    /// 
    /// </summary>
    public void MergeBlock(BlockController otherBlock)
    {
        this.number *= 2;
        Debug.Log("Merged block number:" + this.number);
        UpdateSprite();
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateSprite()
    {
        int index = (int)Mathf.Log(number, 2) - 1;
        if (index >= 0 && index < numberSpriteArray.Length)
        {
            spriteRenderer.sprite = numberSpriteArray[index];
        }
        else
        {
            Debug.LogError("Sprite index out of bounds: " + index + ", for number: " + number);
        }
    }
}
