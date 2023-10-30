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
    public GameColor BotColor { get => botColor; set => botColor = value; }

    GameColor botColor;
    public override void Start()
    {
        base.Start();
        int randomColor = ColorManager.Instance.GetNextColorIndex();
        transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", ColorManager.Instance.colorDataSO.ColorDatas[randomColor].color);
        BotColor = ColorManager.Instance.colorDataSO.ColorDatas[randomColor].colorName;
        destination = GameObject.FindGameObjectWithTag(Tag.DESTINATION);
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
    public void DestroyBrickObject(Transform brick)
    {
        Destroy(brick.gameObject);
    }
}
