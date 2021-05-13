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

    public WeaponData weaponData;

    private Quaternion localStartRotationHorizontal;
    private Quaternion localStartRotationVertical;
    [HideInInspector]
    public GameObject weaponNode;

    float fireCooldown;

    [Header("Effects")]
    public GameObject shootEffect;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Unity Setup Fields")]

    public List<string> enemyTags = new List<string>(new[] { "Enemy" });


    public Transform partToRotateHorizontal;
    public Transform partToRotateVertical;

    public Transform firePoint;
    public Transform fireEffectPoint;


    // Start is called before the first frame update
    void Start()
    {
        localStartRotationHorizontal = partToRotateHorizontal.localRotation;
        localStartRotationVertical = partToRotateVertical.localRotation;
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        fireCooldown = 0f;
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
            float angle = Quaternion.Angle(partToRotateHorizontal.rotation, targetRotation);

            if (angle <= weaponData.fireAngle && fireCooldown + weaponData.fireStartCooldown <= 0f)
            {
                if (weaponData.laserweapon)
                {
                    ShootLaser();
                }
                else
                {
                    ShootAmmo();
                }

                if (weaponData.fireRate > 0f)
                {
                    fireCooldown = (1f / weaponData.fireRate) + weaponData.fireStartCooldown;
                }
            }
        }


    }

    protected void ShootLaser()
    {
        IHittable targetEnemy = this.targetEnemy.GetComponent<IHittable>();

        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(transform.root.gameObject, weaponData.damageOverTime * Time.deltaTime);
            targetEnemy.Slow(transform.root.gameObject, weaponData.slowAmount);
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
        if (weaponData.laserweapon && target == null && lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
            impactEffect.Stop();
            impactLight.enabled = false;
        }

        if (weaponData.fireRate > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    protected void ShootAmmo()
    {
        GameObject ammoGO = Instantiate(weaponData.ammoPrefab, firePoint.position, firePoint.rotation);
        Ammunition ammo = ammoGO.GetComponent<Ammunition>();

        if (ammo != null)
        {
            ammo.Initiate(this.transform.root.gameObject, target, weaponData);

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
            Vector3 rotation = Quaternion.RotateTowards(partToRotateHorizontal.rotation, lookRotation, Time.deltaTime * weaponData.turnRate).eulerAngles;
            partToRotateHorizontal.rotation = Quaternion.Euler(weaponData.xLocked ? 0f : rotation.x, weaponData.yLocked ? 0f : rotation.y, weaponData.zLocked ? 0f : rotation.z);

            Vector3 localRotation = partToRotateHorizontal.localRotation.eulerAngles;

            if (localRotation.y < 180)
            {
                localRotation.y = Mathf.Clamp(localRotation.y, 0, weaponData.turnAngle);
            }
            else
            {
                localRotation.y = Mathf.Clamp(localRotation.y, 360 - weaponData.turnAngle, 360);
            }

            partToRotateHorizontal.localRotation = Quaternion.Euler(weaponData.xLocked ? 0f : localRotation.x, weaponData.yLocked ? 0f : localRotation.y, weaponData.zLocked ? 0f : localRotation.z);

        }
        else
        {
            Vector3 rotation = Quaternion.RotateTowards(partToRotateHorizontal.localRotation, localStartRotationHorizontal, Time.deltaTime * weaponData.turnRate).eulerAngles;
            partToRotateHorizontal.localRotation = Quaternion.Euler(weaponData.xLocked ? 0f : rotation.x, weaponData.yLocked ? 0f : rotation.y, weaponData.zLocked ? 0f : rotation.z);

        }
    }

    void UpdateTarget()
    {
        if (target == null || !weaponData.focusTarget)
        {
            GameObject[] enemies = TargetFinder.FindEnemies(enemyTags);
            GameObject targetEnemy = null;

            targetEnemy = TargetFinder.PickTarget(enemies, transform, weaponData.searchStrategy, weaponData.range, weaponData.turnAngle);

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
            if (distanceToEnemy > weaponData.range || angle > weaponData.turnAngle)
            {
                target = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - partToRotateHorizontal.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Quaternion localLookRotation = Quaternion.Inverse(partToRotateHorizontal.rotation) * lookRotation;
        Vector3 localRotation = Quaternion.Lerp(partToRotateHorizontal.localRotation, localLookRotation, Time.deltaTime * weaponData.turnRate).eulerAngles;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(partToRotateHorizontal.position, partToRotateHorizontal.position + dir);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(partToRotateHorizontal.position, lookRotation * Vector3.forward * 10);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(partToRotateHorizontal.position, partToRotateHorizontal.forward * 10);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponData.range);
    }

}
