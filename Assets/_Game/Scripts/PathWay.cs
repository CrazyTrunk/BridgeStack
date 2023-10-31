using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWay : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] public StairBrick[] stairBricks;
    public void OpenDoor()
    {
        door.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        door.gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
    private void OnEnable()
    {
        StairBrick.OnStairBrickChanged += CheckAndOpenDoor;
    }

    private void OnDisable()
    {
        StairBrick.OnStairBrickChanged -= CheckAndOpenDoor;
    }

    private void CheckAndOpenDoor(StairBrick brick)
    {
        if (AllStairBricksChangedColor())
        {
            OpenDoor();
        }
    }
    public bool AllStairBricksChangedColor()
    {
        foreach (StairBrick brick in stairBricks)
        {
            if (brick.colorName == GameColor.NoColor)
            {
                return false; 
            }
        }
        return true; 
    }
}
