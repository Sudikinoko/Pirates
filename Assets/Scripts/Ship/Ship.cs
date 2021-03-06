using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ship : MonoBehaviour, IHittable
{

    public ShipData shipData;

    public WeaponNode[] weaponNodes;
    public UtilityNode[] utilityNodes;

    public int weaponAmount;
    public int utilityAmount;

    [HideInInspector]
    public float health;
    [HideInInspector]
    public float armor;
    [HideInInspector]
    public float regeneration;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float acceleration;
    [HideInInspector]
    public float turningRate;
    [HideInInspector]
    public float mass;
    [HideInInspector]
    public float drag;
    [HideInInspector]
    public float angularDrag;



    public List<string> enemyTags = new List<string>(new[] { "Enemy" });

    [Header("Effects")]
    public GameObject deathEffect;
    public GameObject waterEffect;
    public GameObject bountyEffect;

    [Header("HealthBar")]
    public GameObject statusBarPrefab;
    private GameObject statusBar;
    private Image healthBarImage;
    public Transform healthBarDisplayPoint;

    private bool isDead = false;
    [HideInInspector]
    public bool constructionMode = false;


    [Header("Physics")]
    private Vector3 movingDirection;
    private Vector3 speedDirection;
    public bool xLocked = false;
    public bool yLocked = false;
    public bool zLocked = false;
    Rigidbody rigidBody;
    MeshCollider meshCollider;
    Vector3 targetPoint;
    AnimationCurve turnHabit;

    [Header("Camera")]
    public Transform zoomOutPosition;
    public Transform zoomInPosition;
    public Transform constructionModePosition;

    private Vector3 lastPosition;

    bool playerControlled = false;

    IController controller;

    // Start is called before the first frame update
    void Start()
    {
        InitiateStartValues();
        InstantiateHealthBar();
        InitiateRigidbody();
        InitiateMeshCollider();
        InstantiateWaterSplashEffect();
        InitiateCameraPoints();
        CheckIfPlayerControlled();
        UpdateHealthBar();

        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - lastPosition).magnitude >= 10)
        {
            lastPosition = transform.position;
            InstantiateWaterSplashEffect();
        }

        Regenerate();
        RenderHealthBar();
        UpdateSpeedBar();
    }

    public void CheckIfPlayerControlled()
    {
        playerControlled = gameObject.GetComponent<PlayerController>() == null ? false : true;
    }

    void InitiateStartValues()
    {
        health = shipData.health;
        armor = shipData.armor;
        regeneration = shipData.regeneration;
        speed = shipData.speed;
        acceleration = shipData.acceleration;
        turningRate = shipData.turningRate;
        mass = shipData.mass;
        drag = shipData.drag;
        angularDrag = shipData.angularDrag;
        turnHabit = shipData.turnHabit;

        controller = GetComponent<IController>();
    }

    void InitiateCameraPoints()
    {
        GameObject constructionPointGO = GameObject.FindGameObjectWithTag("CameraConstructionPoint");
        if (constructionPointGO != null)
        {
            constructionModePosition = constructionPointGO.transform;
        }

        GameObject zoomInPointGO = GameObject.FindGameObjectWithTag("CameraZoomInPoint");
        if (zoomInPointGO != null)
        {
            zoomInPosition = zoomInPointGO.transform;
        }

        GameObject zoomOutPointGO = GameObject.FindGameObjectWithTag("CameraZoomOutPoint");
        if (zoomOutPointGO != null)
        {
            zoomOutPosition = zoomOutPointGO.transform;
        }
    }

    void InstantiateWaterSplashEffect()
    {
        if (waterEffect != null)
        {
            GameObject waterSplash = Instantiate(waterEffect, transform.position, Quaternion.identity);
            Destroy(waterSplash, waterSplash.GetComponent<ParticleSystem>().main.duration);
        }
    }

    public void InitiateRigidbody()
    {
        if (rigidBody == null)
        {
            if (GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
            }

            rigidBody = GetComponent<Rigidbody>();

            rigidBody.mass = mass;
            rigidBody.drag = drag;
            rigidBody.angularDrag = angularDrag;
            rigidBody.useGravity = false;
            rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void InitiateMeshCollider()
    {
        if (meshCollider == null)
        {
            if (GetComponent<MeshCollider>() == null)
            {
                meshCollider = gameObject.AddComponent<MeshCollider>();

                meshCollider.convex = true;
                meshCollider.enabled = true;
            }

        }
        else
        {
            meshCollider.enabled = true;
        }
    }


    void InstantiateHealthBar()
    {
        if (statusBarPrefab != null)
        {
            statusBar = Instantiate(statusBarPrefab, healthBarDisplayPoint != null ? healthBarDisplayPoint.position : healthBarDisplayPoint.position + Vector3.up * 5, transform.rotation);
            statusBar.transform.SetParent(transform);
            healthBarImage = statusBar.GetComponent<StatusBar>().healthBar;

        }
    }

    void RenderHealthBar()
    {
        if (statusBar != null)
        {
            statusBar.transform.LookAt(statusBar.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

    void Regenerate()
    {
        Heal(regeneration * Time.deltaTime);
    }

    void UpdateHealthBar()
    {
        if (playerControlled)
        {
            GameUI.instance.UpdadeHealthBar(health, shipData.health);
            return;
        }

        if (statusBar != null && healthBarImage != null)
        {
            healthBarImage.fillAmount = health / shipData.health;
        }
    }

    void UpdateSpeedBar()
    {
        if (playerControlled && rigidBody != null)
        {
            GameUI.instance.UpdateSpeed(rigidBody.velocity.magnitude);
        }
    }

    public void MoveTo(Vector3 targetPoint, bool shouldAccellerate)
    {
        this.targetPoint = targetPoint;
        Vector3 targetDirection = targetPoint - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        if (!constructionMode && rigidBody != null)
        {
            if (shouldAccellerate)
            {
                Accelerate(targetDirection);
            }

            Turn(targetRotation);
        }
    }

    public void Accelerate(Vector3 targetDirection)
    {
        Vector3 forceDirection = Vector3.ClampMagnitude(Vector3.Project(targetDirection, transform.forward), 100f);
        rigidBody.AddForce(forceDirection * acceleration * Time.fixedDeltaTime, ForceMode.Force);

    }

    public void Accelerate(float amount)
    {
        Vector3 forceDirection = transform.forward;
        rigidBody.AddForce(forceDirection * amount * acceleration * Time.fixedDeltaTime * 100f, ForceMode.Force);

    }

    public void Turn(Quaternion targetRotation)
    {
        float maxSpeedRate = 1 - Mathf.Clamp01(rigidBody.velocity.magnitude / speed);


        Vector3 rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turningRate * turnHabit.Evaluate(maxSpeedRate)).eulerAngles;
        transform.rotation = Quaternion.Euler(xLocked ? 0f : rotation.x, yLocked ? 0f : rotation.y, zLocked ? 0f : rotation.z);

    }


    public void TakeDamage(GameObject damageDealer, float damage)
    {
        float finalDamage = damage - armor;

        if (finalDamage > 0f)
        {
            health -= finalDamage;

            if (health <= 0f && !isDead)
            {
                Die(damageDealer);
            }

        }
        UpdateHealthBar();
        controller.DamageFlag();
    }
    public void Heal(float heal)
    {
        health += heal;

        if (health >= shipData.health)
        {
            health = shipData.health;
        }
        UpdateHealthBar();
    }

    private void Die(GameObject killer)
    {
        isDead = true;

        ICollector collector = killer.GetComponent<ICollector>();
        if (collector != null)
        {
            int bounty = (int)Random.Range(shipData.minValue, shipData.maxValue);
            collector.AddMoney(bounty);
            if (bountyEffect != null)
            {
                GameObject effect = Instantiate(bountyEffect, transform.position, Quaternion.identity);
                effect.GetComponent<BountyUI>().StartBountyAnimation(bounty.ToString());
            }
        }

        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        }

        if (playerControlled)
        {
            Respawn();
            return;
        }

        Destroy(gameObject);

    }

    public void Slow(GameObject damageDealer, float percent)
    {
        throw new System.NotImplementedException();
    }

    private void OnDrawGizmos()
    {
        if (targetPoint == null || targetPoint == Vector3.zero)
            return;

        Vector3 dir = targetPoint - transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + dir);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 50);
    }

    public void Respawn()
    {
        isDead = false;
        gameObject.GetComponent<PlayerController>().Die();
    }
}
