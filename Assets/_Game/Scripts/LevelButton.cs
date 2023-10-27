using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] PlayerDataSO playerDataSO;
    // Start is called before the first frame update
    void Start()
    {
        LoadResource();
    }

    private void LoadResource()
    {
        GameObject player = Resources.Load<GameObject>($"Player");
        if (player != null)
        {
            GameObject currentPlayer = Instantiate(player);
            currentPlayer.transform.position = spawnPoint.position;
            playerDataSO.PlayerData.Position = spawnPoint.position;
        }
        CameraFollow.Instance.Init();
    }
}
