using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    public void OnPlayButtonClick()
    {
        Hide();
        LevelManager.Instance.LoadLevel();
        GameManager.Instance.ChangeState(GameState.Playing);
    }
    public void OnMenuLevelSelected()
    {
        Hide();
        LevelMenu.Show();
    }
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
}
