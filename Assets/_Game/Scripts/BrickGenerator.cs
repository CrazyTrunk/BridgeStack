using System.Collections.Generic;
using UnityEngine;

public class BrickGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform BrickPrefab;
    [SerializeField] public List<BrickSpawnData> BrickSpawnDatas;
    //[SerializeField] public ColorDataSO colorDataSO;
    private int length = 24;// number of bricks
    private Vector3 position;// each Brick Pos
    private int line = 6;
    private int step = 2;

    float zPosInit;
    float xPosInit;

    private void OnInit()
    {
        position = transform.position;
        zPosInit = position.z;
        xPosInit = position.x;
        BrickSpawnDatas = new List<BrickSpawnData>();
    }
    public void SpawnBricks()
    {
        OnInit();
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
        int randomColor = Random.Range(0, ColorManager.Instance.colorDataSO.ColorDatas.Count);
        //_Color Shader Color Properties
        createdBrick.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", ColorManager.Instance.colorDataSO.ColorDatas[randomColor].color);
        createdBrick.GetComponent<Brick>().colorName = ColorManager.Instance.colorDataSO.ColorDatas[randomColor].colorName;
        createdBrick.GetComponent<Brick>().brickNumber = i;
        createdBrick.GetComponent<Brick>().color = ColorManager.Instance.colorDataSO.ColorDatas[randomColor].color;
        InsertBrickSpawnDataToList(ColorManager.Instance.colorDataSO.ColorDatas[randomColor].color, ColorManager.Instance.colorDataSO.ColorDatas[randomColor].colorName, createdBrick);
    }
    public void MakeRemovedBrick(int brickNumber)
    {
        BrickSpawnDatas[brickNumber].removed = true;
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
        BrickSpawnDatas.Add(newBrickData);
    }
    public void RegenerateBricks(GameColor color)
    {
        for(int i = 0; i < BrickSpawnDatas.Count; i++)
        {
            if (BrickSpawnDatas[i].removed == true && BrickSpawnDatas[i].colorName == color)
            {
                Transform createdBrick = Instantiate(BrickPrefab, BrickSpawnDatas[i].position, BrickPrefab.transform.rotation, transform);
                createdBrick.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", BrickSpawnDatas[i].color);
                createdBrick.GetComponent<Brick>().colorName = BrickSpawnDatas[i].colorName;
                createdBrick.GetComponent<Brick>().brickNumber = i;
                createdBrick.GetComponent<Brick>().color = BrickSpawnDatas[i].color;
                BrickSpawnDatas[i].removed = false;
                return;
            }
        }
    }
}
