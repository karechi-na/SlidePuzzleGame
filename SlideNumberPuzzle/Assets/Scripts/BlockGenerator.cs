using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField] private GameObject SquareNo2;
    private BlockController controller;
    private GameDirector gameDirector;


    private void SquareGenerator()
    {
        if (controller.isTransform)
        {
            GameObject square = Instantiate(SquareNo2);
            float coordinateX = Random.Range(-3, 4);
            float coordinateY = Random.Range(-3, 4);
            square.transform.position = new Vector3(coordinateX, coordinateY, 0);
            //controller.isTransform = false;
        }
        
    }
}
