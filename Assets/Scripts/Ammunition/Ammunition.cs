using UnityEngine;

public class Ammunition : MonoBehaviour
{

    GameObject shooter;

    WeaponData weaponData;

    Transform target;
    Vector3 targetPosition;

    public float lifetime;

    public float damage;
    public float speed;

    public float explosionRadius = 0f;
    public GameObject impactEffect;

    public bool isSeeking = true;

    // Update is called once per frame
    public void Update()
    {
        if (isSeeking)
        {
            markTargetPosition();
        }

        MoveToTargetPosition();
    }

    private void MoveToTargetPosition()
    {
        Vector3 dir = targetPosition - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(targetPosition);
    }

    protected void HitTarget()
    {
        ImpactEffect();

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            if (target != null && (isSeeking || (target.position - transform.position).magnitude < 1))
            {
                Damage(target);
            }
        }
        Destroy(gameObject);
    }

    private void ImpactEffect()
    {
        if (impactEffect != null)
        {
            GameObject effectInstance = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, impactEffect.GetComponent<ParticleSystem>().main.duration - 3f);
        }
    }

    private void Damage(Transform enemy)
    {
        IHittable hittable = enemy.GetComponent<IHittable>();

        if (hittable != null)
        {
            hittable.TakeDamage(shooter, damage);
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    public void markTargetPosition()
    {
        if (target != null)
        {
            targetPosition = target.position;
        }
    }

    public void Initiate(GameObject shooter, Transform target, WeaponData weaponData)
    {
        this.shooter = shooter;
        this.target = target;
        this.weaponData = weaponData;
        damage = weaponData.dmg;
        speed = weaponData.bulletSpeed;

        markTargetPosition();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
