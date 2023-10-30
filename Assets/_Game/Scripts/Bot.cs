using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
        StartCoroutine(ChangeStateRandomly());
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
    IEnumerator ChangeStateRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            SetState(new IdleState(this));
            yield return new WaitForSeconds(1f);

            SetState(new PickUpBrickState(this));


        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Brick brick = other.transform.GetComponent<Brick>();

        if (other.CompareTag(Tag.BRICK))
        {
            if (brick.colorName == botColor)
            {
                BrickGenerators[0].MakeRemovedBrick(brick.brickNumber);
                UpdateBotBrick(brick.color);
                Destroy(other.gameObject);

            }
        }
    }
    public void UpdateBotBrick(Color color)
    {
        Transform brick = Instantiate(brickPrefab, brickHolder);
        Vector3 brickPosition = new Vector3(0, 0 + (totalBrick * 0.2f), 0);
        brick.localPosition = brickPosition;
        brick.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        totalBrick++;
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

}
