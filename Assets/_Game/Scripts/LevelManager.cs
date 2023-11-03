using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager> 
{
    public List<Level> levels = new List<Level>();
    [SerializeField] PlayerDataSO playerDataSO;
    private int level = 1;
    Level currentLevel;

    private void Start()
    {
        UIManager.Instance.ShowMenuUI();
    }
    public void LoadLevel()
    {
        LoadLevel(level);
    }
    public void LoadLevel(int index)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        currentLevel = Instantiate(levels[index - 1]);
        LoadPlayerResource();
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
}
