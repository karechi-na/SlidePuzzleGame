using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    [Header("ƒCƒ[ƒW‚Ì”z—ñ(‚O‚©‚ç‡‚É‚Q`‚Q‚O‚S‚W‚Ü‚Å‚¢‚ê‚é)")]
    [SerializeField] private Sprite[] numberSpriteArray = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    private Vector2 startPos;
    public bool isTransform = false;
    private GameDirector gameDirector;
    public Vector2 gridPosition = Vector2.zero;
    public int number { private set; get; } = 0;
    public bool isMerge = false;

    void Start()
    {
        spriteRenderer.sprite = numberSpriteArray[number];
    }

    public void ChangeNextBlockNumber()
    {
        number++;
        spriteRenderer.sprite = numberSpriteArray[number];

        if (number == 11)
        {
            gameDirector.SceneSwitching();
        }
    }

    //¶‚ÉˆÚ“®
    public Tween transformLeft(int moveGridCount)
    {
        return this.transform.DOMove(new Vector3(transform.position.x - (2.0f * moveGridCount), transform.position.y, 0), 0.5f)
                        .OnComplete(() =>
                        {
                            if (isMerge)
                            {
                                Destroy(gameObject);
                            }
                            else
                            {
                                gridPosition.x -= moveGridCount;
                            }
                        });
    }

    //‰E‚ÉˆÚ“®
    public Tween transformRight(int moveGridCount)
    {
        return this.transform.DOMove(new Vector3(transform.position.x + (2.0f * moveGridCount), transform.position.y, 0), 0.5f)
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

    //‰º‚ÉˆÚ“®
    public Tween transformDown(int moveGridCount)
    {
        return this.transform.DOMove(new Vector3(transform.position.x, transform.position.y - (2.0f * moveGridCount), 0), 0.5f)
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

    //ã‚ÉˆÚ“®
    public Tween transformUp(int moveGridCount)
    {
        return this.transform.DOMove(new Vector3(transform.position.x, transform.position.y + (2.0f * moveGridCount), 0), 0.5f)
                        .OnComplete(() =>
                        {
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
