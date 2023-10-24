using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpawnedBricksData", menuName = "_Game/ScriptableObjects/BrickSpawnDataSO", order = 1)]
public class BrickSpawnDataSO : ScriptableObject
{
    public List<BrickSpawnData> BrickSpawnDatas;
}
