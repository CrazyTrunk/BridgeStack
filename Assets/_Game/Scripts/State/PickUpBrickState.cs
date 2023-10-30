using System;
using UnityEngine;

public class PickUpBrickState : IState
{
    private Bot bot;
    private Collider[] detectedObjects = new Collider[10];
    private Transform targetBrick;
    public PickUpBrickState(Bot bot)
    {
        this.bot = bot;
    }
    public void OnEnter()
    {
        targetBrick = DetectBricksAroundBot();
    }

    public void OnExecute()
    {
        if(targetBrick != null)
        {
            UpdateBotBrick();
            bot.DestroyBrickObject(targetBrick);
        }
    }

    public void OnExit()
    {
    }
    private Transform DetectBricksAroundBot()
    {
        int detectedCount = Physics.OverlapSphereNonAlloc(bot.transform.position, 10f, detectedObjects); 

        for (int i = 0; i < detectedCount; i++)
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
    private void UpdateBotBrick()
    {
       
    }
}