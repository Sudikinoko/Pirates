using UnityEngine;
using UnityEngine.EventSystems;

public enum CameraState
{
    ZoomedOut,
    ZoomedIn,
    ConstructMode,
    Shop
}

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    public delegate void CameraUpdateDelegate();
    public static CameraUpdateDelegate cameraUpdate;


    CameraState cameraState = CameraState.ZoomedOut;


    Transform playerTransform;

    Ship playerShip;

    Transform zoomedOutTransform;
    Transform zoomedInTransform;
    Transform constructionModeTransform;
    public Transform shopTransform;

    public float transitionSpeed;
    public float transitionRotationSpeed = 100f;

    public float rotationSpeed = 100f;

    [Range(0.01f, 1.0f)]
    public float smoothFactorZoomedOut = 0.5f;
    [Range(0.01f, 1.0f)]
    public float smoothFactorZoomedIn = 0.01f;
    [Range(0.01f, 1.0f)]
    public float smoothFactorConstruction = 0.5f;
    [Range(0.01f, 1.0f)]
    public float smoothFactorShop = 0.5f;

    Vector3 touchStart;
    Vector3 touchStart2;

    public float zoomOutMin = 1;
    public float zoomOutMax = 800;

    private bool isPaning = true;
    private bool isPanMode = true;
    private bool reachedConstructPoint = false;

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
    }

    public void InitiateShipParameter()
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
        cameraUpdate();

    }

    void Zoom()
    {
        //Nur beim ersten Frame true
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isPaning = true;
        }
        if (Input.touchCount == 2)
        {
            isPaning = false;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            ZoomInOut(difference * 0.1f);
        }
        else if (isPaning && (cameraState != CameraState.ZoomedOut || isPanMode))
        {
            Pan();
        }
        ZoomInOut(Input.GetAxis("Mouse ScrollWheel"));
    }

    void ZoomInOut(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    private void Pan()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
    }


    private void ZoomedOutUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, playerTransform.position + zoomedOutTransform.localPosition * 5f, smoothFactorZoomedOut);
        transform.rotation = Quaternion.Lerp(transform.rotation, zoomedOutTransform.localRotation, transitionRotationSpeed * Time.deltaTime);
        Zoom();
    }
    private void ZoomedInUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, zoomedInTransform.position, smoothFactorZoomedIn);

        Vector3 dir = playerTransform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * transitionRotationSpeed);

    }

    private void ConstructModeUpdate()
    {
        if (!reachedConstructPoint)
        {
            transform.position = Vector3.Lerp(transform.position, constructionModeTransform.position, smoothFactorConstruction);
            transform.rotation = constructionModeTransform.rotation;
            reachedConstructPoint = Vector3.Distance(transform.position, constructionModeTransform.position) <= 1f;
        }
        Zoom();
    }

    private void ShopUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, shopTransform.position + shopTransform.forward * 100f, smoothFactorShop);

        Vector3 dir = shopTransform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * transitionRotationSpeed);
    }


    public void SetCameraState(CameraState cameraState)
    {
        this.cameraState = cameraState;

        switch (cameraState)
        {
            case CameraState.ZoomedOut:
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = playerShip.shipData.cameraZoomedOutSize;
                isPanMode = !isPanMode;
                cameraUpdate = ZoomedOutUpdate;
                break;
            case CameraState.ZoomedIn:
                Camera.main.orthographic = false;
                isPanMode = true;
                cameraUpdate = ZoomedInUpdate;
                break;
            case CameraState.ConstructMode:
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = playerShip.shipData.cameraConstructionSize;
                reachedConstructPoint = false;
                isPanMode = true;
                cameraUpdate = ConstructModeUpdate;
                break;
            case CameraState.Shop:
                Camera.main.orthographic = false;
                isPanMode = true;
                cameraUpdate = ShopUpdate;
                break;
        }
    }

    public void SetPlayer(GameObject player)
    {
        playerTransform = player.transform;

        InitiateShipParameter();
    }

}
