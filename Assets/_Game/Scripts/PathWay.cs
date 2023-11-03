using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class PathWay : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] public StairBrick[] stairBricks;
    [SerializeField] public int groupId;
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
        if (brick.groupId == groupId && AllStairBricksChangedColor(brick.colorName))
        {
            door.OpenDoor();
        }
    }
    public bool AllStairBricksChangedColor(GameColor color)
    {
        foreach (StairBrick brick in stairBricks)
        {
            if (brick.colorName != color)
            {
                return false; 
            }
        }
        return true; 
    }
}
