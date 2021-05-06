using System.Collections.Generic;
using UnityEngine;

public enum SearchStrategy
{
    nearest,
    furthest,
    lowestLife,
    highestLife,
    highestArmor,
    lowestArmor,
    mostValue
}

public class Weapon : MonoBehaviour
{

    private Transform target;
    private Ship targetEnemy;

    private Quaternion localStartRotation;
    public GameObject weaponNode;


    [Header("Attributes")]
    public float range;
    public float dmg;
    public bool focusTarget = true;


    [Range(0f, 180f)]
    public float turnAngle;
    public float turnRate;
    public float accuracy;
    public float fireAngle = 10f;

    public float mass;

    public bool isAutofire;
    public SearchStrategy searchStrategy = SearchStrategy.nearest;

    [Header("Rotation Lock")]
    public bool xLocked = false;
    public bool yLocked = false;
    public bool zLocked = false;

    [Header("Use Bullets (default)")]
    public GameObject ammoPrefab;
    public float fireRate;
    public float fireCooldown;


    [Header("Use Laser")]
    public bool laserweapon = false;

    public float damageOverTime;
    public float slowAmount;

    [Header("Effects")]
    public GameObject shootEffect;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Unity Setup Fields")]

    public List<string> enemyTags = new List<string>(new[] { "Enemy" });


    public Transform partToRotate;

    public Transform firePoint;
    public Transform fireEffectPoint;


    // Start is called before the first frame update
    void Start()
    {
        localStartRotation = partToRotate.localRotation;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    public void Update()
    {
        LockOnTarget();
        ShootIfAimed();
    }

    void ShootIfAimed()
    {
        Reload();

        if (target != null)
        {
            Vector3 targetDirection = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            float angle = Quaternion.Angle(partToRotate.rotation, targetRotation);

            if (angle <= fireAngle && fireCooldown <= 0f)
            {
                if (laserweapon)
                {
                    ShootLaser();
                }
                else
                {
                    ShootAmmo();
                }

                if (fireRate > 0f)
                {
                    fireCooldown = 1f / fireRate;
                }
            }
        }


    }

    protected void ShootLaser()
    {
        IHittable targetEnemy = this.targetEnemy.GetComponent<IHittable>();

        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(transform.root.gameObject, damageOverTime * Time.deltaTime);
            targetEnemy.Slow(transform.root.gameObject, slowAmount);
        }

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 direction = firePoint.position - target.position;

        impactEffect.transform.position = target.position + direction.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(direction);
    }

    protected void Reload()
    {
        if (laserweapon && target == null && lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
            impactEffect.Stop();
            impactLight.enabled = false;
        }

        if (fireRate > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    protected void ShootAmmo()
    {
        GameObject ammoGO = Instantiate(ammoPrefab, firePoint.position, firePoint.rotation);
        Ammunition ammo = ammoGO.GetComponent<Ammunition>();

        if (ammo != null)
        {
            ammo.Initiate(this.transform.root.gameObject, target);

            if (shootEffect != null)
            {
                GameObject effectInstance = Instantiate(shootEffect, fireEffectPoint.position, fireEffectPoint.rotation);
                Destroy(effectInstance, effectInstance.GetComponent<ParticleSystem>().main.duration - 3f);
            }
        }

    }

    void LockOnTarget()
    {
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.RotateTowards(partToRotate.rotation, lookRotation, Time.deltaTime * turnRate).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(xLocked ? 0f : rotation.x, yLocked ? 0f : rotation.y, zLocked ? 0f : rotation.z);

            Vector3 localRotation = partToRotate.localRotation.eulerAngles;

            if (localRotation.y < 180)
            {
                localRotation.y = Mathf.Clamp(localRotation.y, 0, turnAngle);
            }
            else
            {
                localRotation.y = Mathf.Clamp(localRotation.y, 360 - turnAngle, 360);
            }

            partToRotate.localRotation = Quaternion.Euler(xLocked ? 0f : localRotation.x, yLocked ? 0f : localRotation.y, zLocked ? 0f : localRotation.z);

        }
        else
        {
            Vector3 rotation = Quaternion.RotateTowards(partToRotate.localRotation, localStartRotation, Time.deltaTime * turnRate).eulerAngles;
            partToRotate.localRotation = Quaternion.Euler(xLocked ? 0f : rotation.x, yLocked ? 0f : rotation.y, zLocked ? 0f : rotation.z);

        }
    }

    void UpdateTarget()
    {
        if (target == null || !focusTarget)
        {
            GameObject[] enemies = TargetFinder.FindEnemies(enemyTags);
            GameObject targetEnemy = null;

            targetEnemy = TargetFinder.PickTarget(enemies, transform, searchStrategy, range, turnAngle);

            if (targetEnemy != null)
            {
                target = targetEnemy.transform;
                this.targetEnemy = targetEnemy.GetComponent<Ship>();
            }
            else
            {
                target = null;

            }
        }
        else
        {
            float angle = Vector3.Angle(target.transform.position - transform.position, transform.forward);
            float distanceToEnemy = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToEnemy > range || angle > turnAngle)
            {
                target = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - partToRotate.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Quaternion localLookRotation = Quaternion.Inverse(partToRotate.rotation) * lookRotation;
        Vector3 localRotation = Quaternion.Lerp(partToRotate.localRotation, localLookRotation, Time.deltaTime * turnRate).eulerAngles;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(partToRotate.position, partToRotate.position + dir);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(partToRotate.position, lookRotation * Vector3.forward * 10);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(partToRotate.position, partToRotate.forward * 10);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
