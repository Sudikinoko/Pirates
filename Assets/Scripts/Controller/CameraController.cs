using UnityEngine;

public enum CameraState
{
    ZoomedOut,
    ZoomedIn,
    ConstructMode
}

public class CameraController : MonoBehaviour
{

    public delegate void CameraUpdateDelegate();
    public static CameraUpdateDelegate cameraUpdate;


    CameraState cameraState = CameraState.ZoomedOut;

    public Transform playerTransform;

    Ship playerShip;

    Transform zoomedOutTransform;
    Transform zoomedInTransform;
    Transform constructionModeTransform;

    public float transitionSpeed;
    public float transitionRotationSpeed = 100f;

    public float rotationSpeed = 100f;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        InitiateShipParameter();
    }

    void InitiateShipParameter()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        playerShip = playerTransform.GetComponent<Ship>();

        zoomedOutTransform = playerShip.zoomOutPosition;
        zoomedInTransform = playerShip.zoomInPosition;
        constructionModeTransform = playerShip.constructionModePosition;

        SetCameraState(cameraState);

    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetCameraState(CameraState.ZoomedOut);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SetCameraState(CameraState.ZoomedIn);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SetCameraState(CameraState.ConstructMode);
        }

        cameraUpdate();


    }


    private void ZoomOutUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, playerTransform.position + zoomedOutTransform.localPosition * 5f, smoothFactor);


        transform.rotation = Quaternion.Lerp(transform.rotation, zoomedOutTransform.localRotation, transitionRotationSpeed * Time.deltaTime);
    }
    private void ZoomInUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, zoomedInTransform.position, smoothFactor);

        transform.LookAt(playerTransform);

    }

    private void ConstructModeUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, constructionModeTransform.position, smoothFactor);

        transform.rotation = constructionModeTransform.rotation;
    }


    public void SetCameraState(CameraState cameraState)
    {
        this.cameraState = cameraState;

        switch (cameraState)
        {
            case CameraState.ZoomedOut:
                cameraUpdate = ZoomOutUpdate;
                break;
            case CameraState.ZoomedIn:
                cameraUpdate = ZoomInUpdate;
                break;
            case CameraState.ConstructMode:
                cameraUpdate = ConstructModeUpdate;
                break;
        }
    }

    public void SetPlayer(GameObject player)
    {
        playerTransform = player.transform;

        InitiateShipParameter();
    }

}
