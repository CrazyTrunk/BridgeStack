using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairBrick : Brick
{
    [SerializeField] public Renderer brickRenderer;
    public delegate void StairBrickChangedHandler(StairBrick brick);
    public static event StairBrickChangedHandler OnStairBrickChanged;
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
