using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{

    public static MinimapCameraController instance;

    Transform playerTransform;

    [Range(0.01f, 1.0f)]
    public float smoothFactorZoomedOut = 0.5f;

    public float height = 500f;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        InitiateShipParameter();
        transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.right);
        transform.Rotate(0f, 0f, 90f);
    }

    public void InitiateShipParameter()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        ZoomedOutUpdate();
    }



    private void ZoomedOutUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, playerTransform.position + Vector3.up * height, smoothFactorZoomedOut);
    }


    public void SetCamera()
    {
        Camera.main.orthographic = true;
    }

}
