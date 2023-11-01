using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Bot : Character
{
    private GameObject destination;
    [SerializeField] private NavMeshAgent agent;
    private IState currentState;
    private BotData botData;

    public NavMeshAgent Agent { get => agent; set => agent = value; }
    public BotData BotData { get => botData; set => botData = value; }

    public Zone currentZone;
    [SerializeField] private LayerMask stairLayer;
    public LayerMask StairLayer { get => stairLayer; set => stairLayer = value; }
    private bool hasExitedDoor = false;

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

    private void OnTriggerEnter(Collider other)
    {
        Brick brick = other.transform.GetComponent<Brick>();
        if (other.CompareTag(Tag.BRICK))
        {
            if (brick.colorName == BotData.botColor)
            {
                Destroy(other.gameObject);
                BrickGenerators[botData.Zone].MakeRemovedBrick(brick.brickNumber);
                UpdateBotBrick(brick.color);
            }

        }
    }
    public void UpdateBotBrick(Color color)
    {
        Transform brick = Instantiate(brickPrefab, brickHolder);
        Vector3 brickPosition = new Vector3(0, 0 + (totalBrick * 0.2f), 0);
        brick.localPosition = brickPosition;
        brick.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        BotData.totalBrickCollected++;
        totalBrick++;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.DOOR) && !hasExitedDoor)
        {
            hasExitedDoor = true;

            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            BrickGenerators[botData.Zone].SpawnBricks();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Tag.GROUND))
        {
            Zone zone = collision.collider.GetComponent<Zone>();
            if (zone != null)
            {
                botData.Zone = zone.ZoneID;
                currentZone = collision.collider.GetComponent<Zone>();
                SetState(new PlaceBrickOnBridgeState(this));
            }
        }
    }
}
