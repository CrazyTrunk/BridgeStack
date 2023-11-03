using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    public Transform SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
}
