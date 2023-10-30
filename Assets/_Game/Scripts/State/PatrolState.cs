using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class PatrolState : IState
{
    private Bot bot;
    public PatrolState(Bot bot)
    {
        this.bot = bot;
    }
    public void OnEnter()
    {
        bot.ChangeAnim("idle");
        
    }

    public void OnExecute()
    {
        bot.ChangeAnim("run");
    }

    public void OnExit()
    {
    }
}