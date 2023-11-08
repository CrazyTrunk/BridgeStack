using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairBrick : Brick
{
    [SerializeField] public Renderer brickRenderer;
    [SerializeField] public int groupId;
    public Action<StairBrick> OnStairBrickChanged;
    private void Start()
    {
        color = Color.white;
        colorName = GameColor.NoColor;
    }
    public void SetColor(GameColor newColor)
    {
        colorName = newColor;
        OnStairBrickChanged?.Invoke(this);
    }
}
