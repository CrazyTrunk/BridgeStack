using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Level> levels;
    [SerializeField] public LevelButton buttonPrefab;
    [SerializeField] PlayerDataSO playerDataSO;
    private int level = 1;
    Level currentLevel;
    private GameObject currentPlayer;
    private GameObject currentBot;
    public void SpawnButtons(Transform buttonsParent)
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
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }
        if (currentBot != null)
        {
            Destroy(currentBot);
        }
        currentLevel = Instantiate(levels.Find(x => x.Id == level));
        LoadPlayerResource();
        LoadBotResources();
        LevelMenu.Hide();
        GameManager.Instance.ChangeState(GameState.Playing);
    }
    public void NextLevel()
    {
        level++;
        LoadLevel();
    }
    private void LoadPlayerResource()
    {
        GameObject player = Resources.Load<GameObject>($"Player");
        if (player != null)
        {
            currentPlayer = Instantiate(player);
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
            currentBot = Instantiate(bot);
            currentBot.transform.position = currentLevel.SpawnPoint.position;
            playerDataSO.PlayerData.Position = currentLevel.SpawnPoint.position;
        }
    }

}
