using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BrickGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform BrickPrefab;
    [SerializeField] public BrickSpawnDataSO brickSpawnSO;
    [SerializeField] public ColorDataSO colorDataSO;
    private int length = 24;// number of bricks
    private Vector3 position;// each Brick Pos
    private int line = 6;
    private int step = 2;

    float zPosInit;
    float xPosInit;

    private void Start()
    {
        position = transform.position;
        zPosInit = position.z;
        xPosInit = position.x;
        SpawnBricks();
    }
    private void SpawnBricks()
    {
        Vector3 currentPos = new Vector3(xPosInit, position.y, zPosInit);

        for (int i = 0; i < length; i++)
        {
            // Instantiate the brick at the current position
           Transform createdBrick = Instantiate(BrickPrefab, currentPos, BrickPrefab.transform.rotation, transform);

            // Update x position or reset for a new row
            if ((i + 1) % line == 0)
            {
                currentPos.z -= step;
                currentPos.x = xPosInit;
            }
            else
            {
                currentPos.x += step;
            }
        }
    }
}
