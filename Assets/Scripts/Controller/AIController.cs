using UnityEngine;

public enum ShipType
{
    Merchant,
    Marine,
    Pirate,
    Boss
}

public enum BehaviorState
{
    Idle,
    Transport,
    Flee,
    Follow,
    Chase,
    Attack
}

public enum AttackStyle
{
    NoAttack,
    FrontAttack,
    SideAttack
}

public class AIController : MonoBehaviour, IController
{

    Ship controlledShip;

    PlayerController playerController;

    Vector3 targetPoint;

    public float range;

    public ShipType shipType;
    public BehaviorState behaviorState = BehaviorState.Idle;
    public AttackStyle attackStyle = AttackStyle.FrontAttack;

    delegate void FindTargetDelegate();
    FindTargetDelegate FindTarget;

    public float weaponRange = 100f;
    public float warnRange = 200f;
    public float chaseFactor = 5f;

    // Start is called before the first frame update
    void Start()
    {
        controlledShip = gameObject.GetComponent<Ship>();
        FindTarget = Idle;
        behaviorState = BehaviorState.Idle;
        SetBehaviorStatus(behaviorState);
        FindTarget();
        playerController = FindPlayerController();
    }

    private void Update()
    {
        if (behaviorState == BehaviorState.Idle)
        {
            return;
        }

        if (behaviorState == BehaviorState.Attack && (playerController.transform.position - transform.position).magnitude > weaponRange)
        {
            SetBehaviorStatus(BehaviorState.Chase);
        }

        if (behaviorState == BehaviorState.Chase)
        {
            if ((playerController.transform.position - transform.position).magnitude < weaponRange)
            {
                SetBehaviorStatus(BehaviorState.Attack);
            }
            else if ((playerController.transform.position - transform.position).magnitude >= weaponRange * chaseFactor)
            {
                SetBehaviorStatus(BehaviorState.Idle);
            }
        }

        if (controlledShip.health / controlledShip.shipData.health <= 0.3f)
        {
            SetBehaviorStatus(BehaviorState.Flee);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controlledShip != null)
        {
            if (playerController == null)
            {
                playerController = FindPlayerController();
            }


            if (Vector3.Distance(transform.position, targetPoint) < 10f)
            {
                FindTarget();
            }
            controlledShip.MoveTo(targetPoint);
        }
    }

    PlayerController FindPlayerController()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void SetBehaviorStatus(BehaviorState newState)
    {
        behaviorState = newState;
        switch (newState)
        {
            case BehaviorState.Idle:
                FindTarget = Idle;
                break;
            case BehaviorState.Transport:
                FindTarget = Transport;
                break;
            case BehaviorState.Flee:
                FindTarget = Flee;
                break;
            case BehaviorState.Follow:
                FindTarget = Follow;
                break;
            case BehaviorState.Chase:
                FindTarget = Chase;
                break;
            case BehaviorState.Attack:
                FindTarget = Attack;
                break;
            default:
                FindTarget = Idle;
                break;
        }
    }

    void Idle()
    {
        NewRandomTarget();
    }

    void Attack()
    {
        Debug.Log(this.name + " Attack");
        switch (attackStyle)
        {
            case AttackStyle.NoAttack:
                SetBehaviorStatus(BehaviorState.Flee);
                break;
            case AttackStyle.FrontAttack:
                FindFrontAttackPoint();
                break;
            case AttackStyle.SideAttack:
                FindSideAttackPoint();
                break;
            default:
                break;
        }
    }

    float DesiredDistanceToTarget()
    {
        float distanceToTarget = (playerController.transform.position - transform.position).magnitude;
        return distanceToTarget < weaponRange ? distanceToTarget - 1f : weaponRange - 10f;
    }

    void FindFrontAttackPoint()
    {
        targetPoint = ZeroY(playerController.transform.position - (playerController.transform.position - transform.position).normalized * DesiredDistanceToTarget());
    }

    void FindSideAttackPoint()
    {
        Vector3 rideSideOfTarget = playerController.transform.right * DesiredDistanceToTarget();
        Vector3 leftSideOfTarget = playerController.transform.right * DesiredDistanceToTarget();

        float distanceRideSide = (transform.position - rideSideOfTarget).magnitude;
        float distanceLeftSide = (transform.position - leftSideOfTarget).magnitude;


        targetPoint = ZeroY(distanceRideSide <= distanceLeftSide ? rideSideOfTarget : leftSideOfTarget);
    }

    void Chase()
    {
        targetPoint = playerController.transform.position;
    }

    void Follow()
    {

    }

    void Transport()
    {

    }

    void Flee()
    {
        targetPoint = ZeroY(transform.position - playerController.transform.position * 100f);
    }

    void NewRandomTarget()
    {
        targetPoint = transform.position + RandomVector3() * Random.Range(10, range);
    }

    Vector3 RandomVector3()
    {
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    Vector3 ZeroY(Vector3 vector) => new Vector3(vector.x, 0f, vector.z);

    public void DamageFlag()
    {
        SetBehaviorStatus(BehaviorState.Attack);
        FindTarget();

        FindFriendsAndWarnThem(transform.position, warnRange);

    }

    void FindFriendsAndWarnThem(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.root.tag == "Enemy")
            {
                AIController aiController = hitCollider.GetComponent<AIController>();
                if (aiController != null)
                {
                    aiController.SetBehaviorStatus(BehaviorState.Attack);
                    aiController.FindTarget();
                }
            }
        }
    }

}
