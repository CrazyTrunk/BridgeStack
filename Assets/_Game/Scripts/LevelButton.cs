using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] public Button levelButton;
    [SerializeField] public Text textLevelButton;

    public void SetData(int id)
    {
        textLevelButton.text = $"Level {id}";
        levelButton.onClick.AddListener(() =>
        {
            LevelMenu.Instance.LoadLevel(id);
        });
    }
}
