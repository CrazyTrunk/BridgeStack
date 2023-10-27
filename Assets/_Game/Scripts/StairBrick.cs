using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairBrick : Brick
{
    [SerializeField] public Renderer brickRenderer;

    private void Start()
    {
        color = Color.white;
        colorName = GameColor.NoColor;
    }
}
