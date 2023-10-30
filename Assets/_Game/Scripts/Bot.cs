using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    public BotState currentState;
    private GameObject destionation;
    [SerializeField] private NavMeshAgent agent;
    private void Start()
    {
        ChangeAnim("run");
        destionation = GameObject.FindGameObjectWithTag(Tag.DESTINATION);
        agent.SetDestination(destionation.transform.position);
    }
}
