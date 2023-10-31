using System;
using UnityEngine;

public class PickUpBrickState : IState
{
    private Bot bot;
    private Collider[] detectedObjects = new Collider[100];
    private Transform targetBrick;
    float radius = 10f;
    public PickUpBrickState(Bot bot)
    {
        this.bot = bot;
    }
    public void OnEnter()
    {
        bot.ChangeAnim("run");
        if(bot.currentZone != null)
        {
            bot.Agent.isStopped = false;

            bot.Agent.SetDestination(bot.currentZone.transform.position);
        }
    }

    public void OnExecute()
    {
        if (targetBrick == null || Vector3.Distance(bot.Agent.transform.position, targetBrick.position) < 1f)
        {
            targetBrick = FindNearestBrickOfSameColor();
            if (targetBrick != null)
            {
                bot.Agent.SetDestination(targetBrick.position);
            }
            else
            {
                radius += 2;
            }
        }
        if(bot.BotData.totalBrickCollected >= 1)
        {
            bot.SetState(new PlaceBrickOnBridgeState(bot));
        }
    }

    public void OnExit()
    {
        targetBrick = null;
        radius = 10f;
    }
    private Transform FindNearestBrickOfSameColor()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(bot.Agent.transform.position, radius, detectedObjects);
        for (int i = 0; i < numColliders; i++)
        {
            if (detectedObjects[i].CompareTag(Tag.BRICK))
            {
                Brick brick = detectedObjects[i].GetComponent<Brick>();
                if(brick.colorName == bot.BotData.botColor)
                {
                    return detectedObjects[i].transform;
                }
            }
        }
        return null;
    }

}