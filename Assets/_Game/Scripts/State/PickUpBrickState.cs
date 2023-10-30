using System;
using UnityEngine;

public class PickUpBrickState : IState
{
    private Bot bot;
    private Collider[] detectedObjects = new Collider[30];
    private Transform targetBrick;
    float radius = 5f;
    public PickUpBrickState(Bot bot)
    {
        this.bot = bot;
    }
    public void OnEnter()
    {
        bot.ChangeAnim("run");
    }

    public void OnExecute()
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
                if(brick.colorName == bot.BotColor)
                {
                    return detectedObjects[i].transform;
                }
            }
        }
        return null;
    }

}