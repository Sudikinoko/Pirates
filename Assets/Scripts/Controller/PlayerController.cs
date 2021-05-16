using UnityEngine;
using UnityEngine.EventSystems;

enum ControllStatus
{
    Touch,
    UI
}

[RequireComponent(typeof(Ship))]
[RequireComponent(typeof(Stats))]
public class PlayerController : MonoBehaviour, IController
{

    public Base homeBase;

    ShipData shipData;

    Ship ship;

    Rigidbody rigidBody;

    static Plane XZPlane = new Plane(Vector3.up, Vector3.zero);

    private float accelerationUISlider = 0f;

    ControllStatus controllStatus;

    void Start()
    {
        gameObject.tag = "Player";
        ship = GetComponent<Ship>();
        rigidBody = transform.GetComponent<Rigidbody>();
        controllStatus = ControllStatus.UI;
    }

    private void FixedUpdate()
    {
        if (rigidBody == null)
        {
            rigidBody = transform.GetComponent<Rigidbody>();
        }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Vector3 moveTo = GetMousePositionOnXZPlane();
            if (!moveTo.Equals(Vector3.zero))
            {
                ship.MoveTo(moveTo);
            }
        }
        else if (controllStatus == ControllStatus.UI && rigidBody != null)
        {
            ship.Accelerate(accelerationUISlider);
        }
    }

    public void AccelerationSliderChanged(float accelerationPercent)
    {
        accelerationUISlider = accelerationPercent;
    }


    public static Vector3 GetMousePositionOnXZPlane()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (XZPlane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            //Just double check to ensure the y position is exactly zero
            hitPoint.y = 0;
            return hitPoint;
        }
        return Vector3.zero;
    }


    public void EnableConstructionMode()
    {
        Destroy(GetComponent<Rigidbody>());
        GetComponent<MeshCollider>().enabled = false;
        ship.constructionMode = true;
    }

    public void DisableConstructionMode()
    {
        ship.InitiateRigidbody();
        ship.InitiateMeshCollider();
        ship.constructionMode = false;
    }

    public void Die()
    {
        transform.position = homeBase.respawn.position;
        transform.rotation = homeBase.respawn.rotation;
        PlayerStats.instance.RemoveMoney(PlayerStats.instance.GetCurrentMoneyAmount() / 2);
        ship.Heal(ship.shipData.health);

    }


    public void SetHomeBase(Base newHomeBase)
    {
        homeBase = newHomeBase;
    }

    public void SetCurrentShip(ShipData shipData)
    {
        this.shipData = shipData;
    }

    public void DamageFlag()
    {
    }
}
