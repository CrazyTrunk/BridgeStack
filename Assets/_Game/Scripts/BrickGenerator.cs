using System.Collections.Generic;
using UnityEngine;

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
        brickSpawnSO.BrickSpawnDatas = new List<BrickSpawnData>();
    }
    public void SpawnBricks()
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
            GiveColorBrick(createdBrick, i);

        }
    }

    private void GiveColorBrick(Transform createdBrick, int i)
    {
        int randomColor = Random.Range(0, colorDataSO.ColorDatas.Count);
        //_Color Shader Color Properties
        createdBrick.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", colorDataSO.ColorDatas[randomColor].color);
        createdBrick.GetComponent<Brick>().colorName = colorDataSO.ColorDatas[randomColor].colorName;
        createdBrick.GetComponent<Brick>().brickNumber = i;
        createdBrick.GetComponent<Brick>().color = colorDataSO.ColorDatas[randomColor].color;
        InsertBrickSpawnDataToList(colorDataSO.ColorDatas[randomColor].color, colorDataSO.ColorDatas[randomColor].colorName, createdBrick);
    }
    public void MakeRemovedBrick(int brickNumber)
    {
        brickSpawnSO.BrickSpawnDatas[brickNumber].removed = true;
    }

    private void InsertBrickSpawnDataToList(Color color, GameColor colorName, Transform createdBrick)
    {
        BrickSpawnData newBrickData = new()
        {
            color = color,
            colorName = colorName,
            position = createdBrick.position,
            removed = false
        };
        brickSpawnSO.BrickSpawnDatas.Add(newBrickData);
    }
    public void RegenerateBricks()
    {
        for(int i = 0; i < brickSpawnSO.BrickSpawnDatas.Count; i++)
        {
            if (brickSpawnSO.BrickSpawnDatas[i].removed == true)
            {
                Transform createdBrick = Instantiate(BrickPrefab, brickSpawnSO.BrickSpawnDatas[i].position, BrickPrefab.transform.rotation, transform);
                createdBrick.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", brickSpawnSO.BrickSpawnDatas[i].color);
                createdBrick.GetComponent<Brick>().colorName = brickSpawnSO.BrickSpawnDatas[i].colorName;
                createdBrick.GetComponent<Brick>().brickNumber = i;
                createdBrick.GetComponent<Brick>().color = brickSpawnSO.BrickSpawnDatas[i].color;
                brickSpawnSO.BrickSpawnDatas[i].removed = false;
                return;
            }
        }
    }
}
