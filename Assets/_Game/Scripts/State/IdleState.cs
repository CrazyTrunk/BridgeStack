using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private Bot bot;
    public IdleState(Bot bot)
    {
        this.bot = bot;
    }
    public void OnEnter()
    {
        bot.ChangeAnim("idle");
    }

    public void OnExecute()
    {
    }

    public void OnExit()
    {
    }
   

}