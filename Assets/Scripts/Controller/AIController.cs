using UnityEngine;

public class AIController : MonoBehaviour
{

    Ship controlledShip;

    Vector3 targetPoint;

    public float range;

    // Start is called before the first frame update
    void Start()
    {
        controlledShip = gameObject.GetComponent<Ship>();
        NewRandomTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlledShip != null)
        {
            if (Vector3.Distance(transform.position, targetPoint) < 10f)
            {
                NewRandomTarget();
            }
            controlledShip.MoveTo(targetPoint);
        }
    }

    void NewRandomTarget()
    {
        targetPoint = transform.position + RandomVector3() * Random.Range(10, range);
    }

    Vector3 RandomVector3()
    {
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

}
