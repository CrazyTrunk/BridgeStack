using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private GameObject destination;
    [SerializeField] private NavMeshAgent agent;
    private IState currentState;
    [SerializeField] private Transform zone;
    public NavMeshAgent Agent { get => agent; set => agent = value; }
    GameColor botColor;
    private void Start()
    {
        int randomColor = ColorManager.Instance.GetNextColorIndex();
        transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", ColorManager.Instance.colorDataSO.ColorDatas[randomColor].color);
        botColor = ColorManager.Instance.colorDataSO.ColorDatas[randomColor].colorName;
        ChangeAnim("run");
        destination = GameObject.FindGameObjectWithTag(Tag.DESTINATION);
        //Agent.SetDestination(destionation.transform.position);
        StartCoroutine(FindBrickRoutine());
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
    private IEnumerator FindBrickRoutine()
    {
        while (true) 
        {
            yield return new WaitForSeconds(1f);

            SetState(new PickUpBrickState(this));
        }
    }
}
