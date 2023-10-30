using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private GameObject destionation;
    [SerializeField] private NavMeshAgent agent;
    private IState currentState;
    [SerializeField] private Transform zone;
    public NavMeshAgent Agent { get => agent; set => agent = value; }

    private void Start()
    {
        ChangeAnim("run");
        destionation = GameObject.FindGameObjectWithTag(Tag.DESTINATION);
        //Agent.SetDestination(destionation.transform.position);
    }
    private void Update()
    {
        currentState?.OnExecute();
    }
    public void SetState(IState newState)
    {
        currentState?.OnExit();

        currentState = newState;
        currentState?.OnEnter();
    }

}
