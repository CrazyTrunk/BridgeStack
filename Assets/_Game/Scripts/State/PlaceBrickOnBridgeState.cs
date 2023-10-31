using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PlaceBrickOnBridgeState : IState
{
    private Bot bot;
    public PlaceBrickOnBridgeState(Bot bot)
    {
        this.bot = bot;

    }
    public void OnEnter()
    {
        bot.ChangeAnim("run");
    }
    public void OnExecute()
    {
        if (!CheckBridgeStair())
        {
            bot.Agent.isStopped = true;
            return;
        }


        if (bot.totalBrick > 0)
        {
            PathWay pathway = bot.currentZone.Bridges[0].GetComponent<PathWay>();
            GameObject go = bot.currentZone.Bridges[0];
            GameObject doorObject = go.GetComponentsInChildren<Transform>()
            .FirstOrDefault(child => child.CompareTag("Door"))?.gameObject;

            if (doorObject != null)
            {
                bot.Agent.SetDestination(doorObject.transform.position);
            }
        }
        else
        {
            RegenerateBrick(bot.BotData.Zone);
            bot.Agent.SetDestination(bot.currentZone.transform.position);
            bot.SetState(new PickUpBrickState(bot));

        }
    }

    public void OnExit()
    {
    }
    private bool CheckBridgeStair()
    {
        RaycastHit hit;

        if (Physics.Raycast(bot.brickPlacer.position + Vector3.up * 2f, Vector3.down, out hit, Mathf.Infinity, bot.StairLayer))
        {
            StairBrick brick = hit.collider.gameObject.GetComponent<StairBrick>();

            if ((bot.totalBrick > 0 && brick.colorName == GameColor.NoColor) || (bot.totalBrick > 0 && brick.colorName != bot.BotData.botColor))
            {
                ColorData currentPlayerColorData = FindColorDataByGameColor(bot.BotData.botColor);
                brick.brickRenderer.material.color = currentPlayerColorData.color;
                brick.colorName = currentPlayerColorData.colorName;
                brick.color = currentPlayerColorData.color;
                brick.brickRenderer.enabled = true;
                brick.pathway.BrickPlaced++;
                bot.RemoveBrick();
                //if (brick.pathway.BrickPlaced == brick.pathway.TotalStair)
                //{
                //    brick.pathway.OpenDoor();
                //}
                return true;
            }
            if ((brick.colorName == GameColor.NoColor && bot.totalBrick == 0) || (bot.totalBrick == 0 && brick.colorName != bot.BotData.botColor))
            {
                return false;
            }
        }
        return true;
    }
    private void RegenerateBrick(int zone)
    {
        bot.BrickGenerators[zone].RegenerateBricks(bot.BotData.botColor);
    }
    public ColorData FindColorDataByGameColor(GameColor targetColor)
    {
        foreach (ColorData data in ColorManager.Instance.colorDataSO.ColorDatas)
        {
            if (data.colorName == targetColor)
            {
                return data;
            }
        }
        return null;
    }
}
