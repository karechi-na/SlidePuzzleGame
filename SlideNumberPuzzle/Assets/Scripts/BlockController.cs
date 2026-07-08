using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    [Header("イメージの配列(0から順に2～2048までいれる)")]
    [SerializeField] private Sprite[] numberSpriteArray = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    public Vector2 gridPosition = Vector2.zero;
    public int number { private set; get; } = 0;

    public bool isMerge = false;

    private Vector2 startPos;
    public bool isTransform = false;
    private GameDirector gameDirector;

    void Start()
    {
        spriteRenderer.sprite = numberSpriteArray[number];
        //UpdateSprite();
    }

    public void ChangeNextBlockNumber()
    {
        number++;
        spriteRenderer.sprite = numberSpriteArray[number];

        if (number == 11)
        {
            gameDirector.SceneSwitching();
        }

        //UpdateSprite();
    }

    //------------------------------
    // ブロックの移動
    //------------------------------

    //左に移動
    public Tween transformLeft(int moveGridCount)
    {
        return this.transform.DOMove(
            new Vector3(transform.position.x - (2.0f * moveGridCount), transform.position.y, 0)
            , 0.25f
            ).OnComplete(() =>
                        {
                            if (isMerge)
                                Destroy(gameObject);
                        });
    }

    //右に移動
    public Tween transformRight(int moveGridCount)
    {
        return this.transform.DOMove(
            new Vector3(transform.position.x + (2.0f * moveGridCount), transform.position.y, 0)
            , 0.25f
            ).OnComplete(() =>
            {
                if (isMerge)
                    Destroy(gameObject);
            });
    }

    //下に移動
    public Tween transformDown(int moveGridCount)
    {
        return this.transform.DOMove(
            new Vector3(transform.position.x, transform.position.y - (2.0f * moveGridCount), 0)
            , 0.25f
            ).OnComplete(() =>
            {
                if (isMerge)
                    Destroy(gameObject);
            });
    }

    //上に移動
    public Tween transformUp(int moveGridCount)
    {
        return this.transform.DOMove(
            new Vector3(transform.position.x, transform.position.y + (2.0f * moveGridCount), 0)
            , 0.25f
            ).OnComplete(() =>
            {
                if (isMerge)
                    Destroy(gameObject);
            });
    }

    public void MergeBlock(BlockController otherBlock)
    {
        this.number *= 2;
        Debug.Log("Merged block number:" + this.number);
        UpdateSprite();
    }

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
