using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairBrick : Brick
{
    [SerializeField] public Renderer brickRenderer;
    public PathWay pathway;
    private void Start()
    {
        color = Color.white;
        colorName = GameColor.NoColor;
    }
}
