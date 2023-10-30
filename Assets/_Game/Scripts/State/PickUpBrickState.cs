using System;
using UnityEngine;

public class PickUpBrickState : IState
{
    private Bot bot;
    private GameObject targetBrick;

    public PickUpBrickState(Bot bot)
    {
        this.bot = bot;
    }
    public void OnEnter()
    {
        targetBrick = FindNearestBrickWithTag(Tag.BRICK);
        if (targetBrick != null)
        {
            bot.Agent.SetDestination(targetBrick.transform.position);
        }
        float distance = Vector3.Distance(bot.transform.position, targetBrick.transform.position);
        if (distance <= 1.0f) 
        {
            //PickupBrick(targetBrick);
            // Transition to another state
            // bot.SetState(new IdleState(bot));
        }
    }

    public void OnExecute()
    {
        if (targetBrick == null)
        {
            return;
        }
    }

    public void OnExit()
    {
    }
    private GameObject FindNearestBrickWithTag(string tag)
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject brick in bricks)
        {
            float distance = Vector3.Distance(bot.transform.position, brick.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = brick;
            }
        }

        return closest;
    }

}