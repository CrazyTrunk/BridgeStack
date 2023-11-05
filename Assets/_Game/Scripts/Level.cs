using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    public int Id;
    public Transform SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
}
