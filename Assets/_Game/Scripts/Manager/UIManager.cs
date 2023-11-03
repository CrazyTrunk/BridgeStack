using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject finishMenu;
    public void ShowMenuUI()
    {
        mainMenu.SetActive(true);
        finishMenu.SetActive(false);
    }
    public void OnButtonPlayClick() {
        mainMenu.SetActive(false);
        LevelManager.Instance.LoadLevel();
    }
}
