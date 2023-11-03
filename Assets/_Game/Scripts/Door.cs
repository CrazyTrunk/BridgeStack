using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField]private NavMeshObstacle Obstacle;

    public void OpenDoor()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        transform.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        Obstacle.enabled = false;
        Obstacle.carving = false;
    }
    public void CloseDoor()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        transform.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        Obstacle.enabled = true;
        Obstacle.carving = true;
    }
}
