using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FinishMenu.Show();

        if (other.CompareTag(Tag.BOT))
        {
            FinishMenu.Instance.SetWinningState(false);
        }
        else
        {
            FinishMenu.Instance.SetWinningState(true);
        }
        GameManager.Instance.ChangeState(GameState.MainMenu);

    }
}
