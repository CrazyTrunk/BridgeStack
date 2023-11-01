using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class PathWay : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] public StairBrick[] stairBricks;
    public void OpenDoor()
    {
        door.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        door.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        //UpdateNavMesh();
    }
    //void UpdateNavMesh()
    //{
    //    NavMeshSurface surface = FindFirstObjectByType<NavMeshSurface>(); 
    //    if (surface == null) return;

    //    surface.BuildNavMesh();
    //}
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
        if (AllStairBricksChangedColor(brick.colorName))
        {
            OpenDoor();
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
