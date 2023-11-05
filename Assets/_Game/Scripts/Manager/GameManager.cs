using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState _gameState;
    private void Awake()
    {
        ChangeState(GameState.MainMenu);
    }
    public void ChangeState(GameState state)
    {
        _gameState = state;
    }
    public bool IsState(GameState gameState)
    {
        return _gameState == gameState;
    }
}
