using UnityEngine;

[RequireComponent(typeof(Ship))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    Ship ship;

    Rigidbody rigidBody;

    static Plane XZPlane = new Plane(Vector3.up, Vector3.zero);


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        ship = GetComponent<Ship>();
        rigidBody = transform.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ship.MoveTo(GetMousePositionOnXZPlane());
        }
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
        ship.constructionMode = true;
    }

    public void DisableConstructionMode()
    {
        ship.InitiateRigidbody();
        ship.constructionMode = false;
    }

}
