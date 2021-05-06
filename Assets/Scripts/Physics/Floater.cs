using UnityEngine;

public class Floater : MonoBehaviour
{

    Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private void Start()
    {
        rigidBody = transform.root.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        float waveheight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if (transform.position.y < waveheight)
        {
            float displacementMultiplier = Mathf.Clamp01(waveheight - transform.position.y / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.deltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}
