using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    Transform target;
    public Vector3 offset;
    public float Speed = 20;

    public void Init()
    {
        Player playerInstance = FindObjectOfType<Player>();
        if (playerInstance != null)
        {
            target = playerInstance.transform;
        }
        else
        {
            Debug.Log("Player object not found in the scene.");
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * Speed);
    }
}
