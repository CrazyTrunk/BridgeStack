using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWay : MonoBehaviour
{
    [SerializeField] private int totalStair;
    [SerializeField] private Transform door;
    private int brickPlaced;

    public int BrickPlaced { get => brickPlaced; set => brickPlaced = value; }
    public int TotalStair { get => totalStair; set => totalStair = value; }

    public void OpenDoor()
    {
        door.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        door.gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
}
