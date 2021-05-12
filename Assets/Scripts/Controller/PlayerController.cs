using UnityEngine;

[RequireComponent(typeof(Ship))]
[RequireComponent(typeof(Stats))]
public class PlayerController : MonoBehaviour
{

    Ship ship;

    Rigidbody rigidBody;

    static Plane XZPlane = new Plane(Vector3.up, Vector3.zero);


    void Start()
    {
        gameObject.tag = "Player";
        ship = GetComponent<Ship>();
        rigidBody = transform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 moveTo = GetMousePositionOnXZPlane();
            if (!moveTo.Equals(Vector3.zero))
            {
                ship.MoveTo(moveTo);
            }
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
        GetComponent<MeshCollider>().enabled = false;
        ship.constructionMode = true;
    }

    public void DisableConstructionMode()
    {
        ship.InitiateRigidbody();
        ship.InitiateMeshCollider();
        ship.constructionMode = false;
    }

}
