using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Bot : Character
{
    [SerializeField] private NavMeshAgent agent;
    private IState currentState;
    private BotData botData;

    public NavMeshAgent Agent { get => agent; set => agent = value; }
    public BotData BotData { get => botData; set => botData = value; }
    private bool hasExitedDoor = false;

    [SerializeField]public Zone currentZone;
    [SerializeField] private LayerMask stairLayer;
    public LayerMask StairLayer { get => stairLayer; set => stairLayer = value; }

    public override void Start()
    {
        base.Start();
        Oninit();
        SetState(new PickUpBrickState(this));
    }
    private void Oninit()
    {
        botData = new BotData();
        int randomColor = ColorManager.Instance.GetNextColorIndex();
        transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", ColorManager.Instance.colorDataSO.ColorDatas[randomColor].color);
        BotData.botColor = ColorManager.Instance.colorDataSO.ColorDatas[randomColor].colorName;
        botData.Zone = 0;
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
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tag.GROUND))
        {
            Zone zone = other.GetComponent<Zone>();

            if (zone != null)
            {
                currentZone = zone;
                botData.Zone = zone.ZoneID;
                if (!zone.IsSpawned)
                {
                    zone.IsSpawned = true;
                    BrickGenerators[botData.Zone].SpawnBricks();
                }
                else
                {
                    hasExitedDoor = false;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Brick brick = other.transform.GetComponent<Brick>();

        if (other.CompareTag(Tag.BRICK))
        {
            if (brick.colorName == BotData.botColor)
            {
                Destroy(other.gameObject);
                BrickGenerators[botData.Zone].MakeRemovedBrick(brick.brickNumber);
                AddBrick(brick.color);
            }
        }
    }
    protected override void AddBrick(Color color)
    {
        base.AddBrick(color);
        BotData.totalBrickCollected++;
        Debug.Log($"Bot {BotData.totalBrickCollected}");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.DOOR) && !hasExitedDoor)
        {
            hasExitedDoor = true;

            Door door = other.transform.GetComponent<Door>();
            if (door != null)
            {
                door.CloseDoor();
            }
        }

    }

}
