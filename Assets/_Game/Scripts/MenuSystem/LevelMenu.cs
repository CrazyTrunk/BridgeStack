using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : Menu<LevelMenu>
{
    [SerializeField] private Transform parentButton;
    protected override void Awake()
    {
        base.Awake();
        LevelManager.Instance.SpawnButtons(parentButton);
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
