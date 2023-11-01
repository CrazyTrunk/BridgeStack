using UnityEngine;

public class PickUpBrickState : IState
{
    private Bot bot;
    private Transform targetBrick;
    float radius = 5f;
    float brickRange = 1;
    public PickUpBrickState(Bot bot)
    {
        this.bot = bot;
    }
    public void OnEnter()
    {
        bot.ChangeAnim("run");
        bot.Agent.isStopped = false;
        brickRange = Random.Range(1, 5);
    }

    public void OnExecute()
    {
        if (targetBrick == null || Vector3.Distance(bot.Agent.transform.position, targetBrick.position) < 0.1f)
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
        if(bot.totalBrick >= brickRange)
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
        //int numColliders = Physics.OverlapSphereNonAlloc(bot.Agent.transform.position, radius, detectedObjects);
        Collider[] numColliders = Physics.OverlapSphere(bot.Agent.transform.position, radius);

        for (int i = 0; i < numColliders.Length; i++)
        {
            if (numColliders[i].CompareTag(Tag.BRICK))
            {
                Brick brick = numColliders[i].GetComponent<Brick>();
                if(brick.colorName == bot.BotData.botColor)
                {
                    return numColliders[i].transform;
                }
            }
        }
        return null;
    }

}