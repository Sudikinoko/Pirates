using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour, IHittable
{
    public WeaponNode[] weaponNodes;
    public UtilityNode[] utilityNodes;

    public int weaponAmount;
    public int utilityAmount;

    public float startHealth = 10f;
    public float startArmor = 0f;
    public float startRegeneration = 0f;
    public float startSpeed = 30f;
    public float startAcceleration = 10f;
    public float startTurningRate = 0.5f;
    public float startMass = 1f;
    public float startDrag = 0.3f;
    public float startAngularDrag = 0.5f;


    float health;
    float armor;
    float regeneration;
    float speed;
    float acceleration;
    float turningRate;
    float mass;
    float drag;
    float angularDrag;

    public float value; //Price | LootAmount,...

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
    Vector3 targetPoint;
    public AnimationCurve turnHabit;

    [Header("Camera")]
    public Transform zoomOutPosition;
    public Transform zoomInPosition;
    public Transform constructionModePosition;

    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        InitiateStartValues();
        InstantiateHealthBar();
        InitiateRigidbody();
        InstantiateWaterSplashEffect();
        InitiateCameraPoints();

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

        RenderHealthBar();
    }

    void InitiateStartValues()
    {
        health = startHealth;
        armor = startArmor;
        regeneration = startRegeneration;
        speed = startSpeed;
        acceleration = startAcceleration;
        turningRate = startTurningRate;
        mass = startMass;
        drag = startDrag;
        angularDrag = startAngularDrag;
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

    void UpdateHealthBar()
    {

        if (statusBar != null && healthBarImage != null)
        {
            healthBarImage.fillAmount = health / startHealth;
        }
    }

    public void MoveTo(Vector3 targetPoint)
    {
        this.targetPoint = targetPoint;
        Vector3 targetDirection = targetPoint - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        if (!constructionMode)
        {
            Accelerate(targetDirection);

            Turn(targetRotation);
        }
    }

    public void Accelerate(Vector3 targetDirection)
    {
        Vector3 forceDirection = Vector3.ClampMagnitude(Vector3.Project(targetDirection, transform.forward), 100f);
        rigidBody.AddForce(forceDirection * acceleration * Time.fixedDeltaTime, ForceMode.Force);

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
    }
    public void Heal(float heal)
    {
        health += heal;

        if (health >= startHealth)
        {
            health = startHealth;
        }
        UpdateHealthBar();
    }

    private void Die(GameObject killer)
    {
        isDead = true;

        ICollector collector = killer.GetComponent<ICollector>();
        if (collector != null)
        {
            collector.AddMoney(value);
            if (bountyEffect != null)
            {
                GameObject effect = Instantiate(bountyEffect, transform.position, Quaternion.identity);
                effect.GetComponent<BountyUI>().StartBountyAnimation(value.ToString());
            }
        }

        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
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
}
