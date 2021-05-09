using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Ship,
    Building,
    Defence
}

[CreateAssetMenu(menuName = "Equipment/New Weapon")]
public class WeaponData : EquipmentData
{
    public AttackType attackType = AttackType.Ship;

    [Header("Attributes")]
    public float range = 50f;
    public float dmg = 1f;
    public bool focusTarget = true;


    [Range(0f, 180f)]
    public float turnAngle = 90f;
    public float turnRate = 20f;
    public float accuracy = 1f;
    public float fireAngle = 10f;

    public float mass = 1f;

    public bool isAutofire = true;
    public SearchStrategy searchStrategy = SearchStrategy.nearest;

    [Header("Rotation Lock")]
    public bool xLocked = false;
    public bool yLocked = false;
    public bool zLocked = false;

    [Header("Use Bullets (default)")]
    public GameObject ammoPrefab;
    public float fireRate = 1f;
    public float fireStartCooldown = 0f;


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

}
