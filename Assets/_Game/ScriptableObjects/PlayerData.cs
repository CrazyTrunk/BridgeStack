using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Vector3 Position;
    public int Zone;
    public GameColor playerColor;
    public int totalBrickCollected;
    public bool isBot = false;
    public int currentLevel = 0;
}
