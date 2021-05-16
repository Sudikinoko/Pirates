using UnityEngine;

public class Pointer_Manager : MonoBehaviour
{

    public Transform lastVisitedBase;

    [SerializeField]
    private Window_QuestPointer pointer;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = new Vector3(lastVisitedBase.position.x, 0f, lastVisitedBase.position.y);
        pointer.Show(position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
