using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : Menu<LevelMenu>
{

    [SerializeField] private List<Level> levels;
    [SerializeField] public Transform buttonsParent;
    [SerializeField] public LevelButton buttonPrefab;
    [SerializeField] PlayerDataSO playerDataSO;
    private int level = 1;
    Level currentLevel;
    protected override void Awake()
    {
        base.Awake();
        SpawnButtons();
    }
    private void SpawnButtons()
    {
        foreach (var item in levels)
        {
            LevelButton levelButton = Instantiate(buttonPrefab, buttonsParent);
            levelButton.name = $"Level {item.Id}";
            levelButton.SetData(item.Id);
        }
    }

    public void LoadLevel()
    {
        LoadLevel(level);
    }
    public void LoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        currentLevel = Instantiate(levels.Find(x=>x.Id == level));
        LoadPlayerResource();
        LoadBotResources();
        Hide();
        GameManager.Instance.ChangeState(GameState.Playing);
    }
    private void LoadPlayerResource()
    {
        GameObject player = Resources.Load<GameObject>($"Player");
        if (player != null)
        {
            GameObject currentPlayer = Instantiate(player);
            currentPlayer.transform.position = currentLevel.SpawnPoint.position;
            playerDataSO.PlayerData.Position = currentLevel.SpawnPoint.position;
        }
        CameraFollow.Instance.Init();
    }
    private void LoadBotResources()
    {
        GameObject bot = Resources.Load<GameObject>($"Bot");
        if (bot != null)
        {
            GameObject currentBot = Instantiate(bot);
            currentBot.transform.position = currentLevel.SpawnPoint.position;
            playerDataSO.PlayerData.Position = currentLevel.SpawnPoint.position;
        }
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
