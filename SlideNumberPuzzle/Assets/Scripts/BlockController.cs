using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    private Vector2 startPos;
    public bool isTransform = false;
    private GameDirector gameDirector;
    public Vector2 gridPosition = Vector2.zero;

    void Start()
    {

    }


    



    //¶‚ÉˆÚ“®
    public void transformLeft(int moveGridCount)
    {
        this.transform.DOMove(new Vector3(transform.position.x - (2.0f * moveGridCount), transform.position.y, 0), 0.5f)
                        .OnComplete(() =>
                        {
                            gridPosition.x -= moveGridCount;
                        });
    }

    //‰E‚ÉˆÚ“®
    public void transformRight(int moveGridCount)
    {
        this.transform.DOMove(new Vector3(transform.position.x + (2.0f * moveGridCount), transform.position.y, 0), 0.5f)
                    .OnComplete(() =>
                     {
                         gridPosition.x += moveGridCount;
                     });
    }

    //‰º‚ÉˆÚ“®
    public void transformDown(int moveGridCount)
    {
        this.transform.DOMove(new Vector3(transform.position.x, transform.position.y - (2.0f * moveGridCount), 0), 0.5f)
            .OnComplete(() =>
            {
                gridPosition.y += moveGridCount;
            });
    }

    //ã‚ÉˆÚ“®
    public void transformUp(int moveGridCount)
    {
        this.transform.DOMove(new Vector3(transform.position.x, transform.position.y + (2.0f * moveGridCount), 0), 0.5f)
                        .OnComplete(() =>
                        {
                            gridPosition.y -= moveGridCount;
                        });
    }
}
