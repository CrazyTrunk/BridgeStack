using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    Transform target;
    public Vector3 offset;
    public float Speed = 20;
    public Vector3 velocity = Vector3.zero;

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

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity,0.4f);
    }
}
