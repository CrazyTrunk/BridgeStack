using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    public void OnPlayButtonClick()
    {
        Hide();
        LevelMenu.Show();
        LevelMenu.Instance.LoadLevel();
        LevelMenu.Hide();
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
