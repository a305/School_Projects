using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFlight : MonoBehaviour {

    public TheWorld world;
    public float sensitivity;
    GameObject root;
    float x = 0;
    float y = 0;
    Vector3 mousepos;
    bool cameraMode = false;
    Vector3 oldpos;
    Quaternion oldrot;

    private float accelerationSpeed = 1;         // World Units per second
    private float rotationAccelerationSpeed = 10; // Degrees per second;

    private Vector3 rotation = Vector3.zero;

    float timeSinceLastDust = 0;
    Queue<GameObject> thrustDust = new Queue<GameObject>();

    private Vector2 lastMousePos;
    private float rotationSpeed = 80;
    private float distance = 10;

    // Use this for initialization
    void Start () {
        root = world.TheRoot.gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (world.isPlayTime == false)
            return;

        if (world.TheRoot == null)
            return;

        root = world.TheRoot.gameObject;

        if (Input.GetKeyDown(KeyCode.C))
        {
            rotation = Vector3.zero;
            cameraMode = !cameraMode;
            if (cameraMode)
            {
                oldpos = transform.localPosition;
                oldrot = transform.localRotation;
            }
            else
            {
                transform.localPosition = oldpos;
                transform.localRotation = oldrot;
            }
        }

        if (cameraMode == false)
            FixedCamera();
        else
        {
            OrbitCamera();
            ThirdPerson();
        }
    }

    private void FixedCamera()
    {
        mousepos = GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
        mousepos.x -= .5f;
        mousepos.y -= .5f;

        if (mousepos.magnitude > .05f)
        {
            x = mousepos.x * -sensitivity;
            y = mousepos.y * sensitivity;

            root.transform.localRotation *= Quaternion.AngleAxis(x, Vector3.forward);
            root.transform.localRotation *= Quaternion.AngleAxis(y, Vector3.right);
        }

        if (Input.GetKey(KeyCode.Q))
            root.transform.localRotation *= Quaternion.AngleAxis(sensitivity / 5, Vector3.up);
        if (Input.GetKey(KeyCode.E))
            root.transform.localRotation *= Quaternion.AngleAxis(-sensitivity / 5, Vector3.up);
    }

    private void OrbitCamera()
    {
        if (Input.GetKey(KeyCode.W))
            rotation.y += Time.deltaTime * rotationAccelerationSpeed;

        if (Input.GetKey(KeyCode.S))
            rotation.y -= Time.deltaTime * rotationAccelerationSpeed;

        if (Input.GetKey(KeyCode.A))
            rotation.x -= Time.deltaTime * rotationAccelerationSpeed;

        if (Input.GetKey(KeyCode.D))
            rotation.x += Time.deltaTime * rotationAccelerationSpeed;

        if (Input.GetKey(KeyCode.E))
            rotation.z -= Time.deltaTime * rotationAccelerationSpeed;

        if (Input.GetKey(KeyCode.Q))
            rotation.z += Time.deltaTime * rotationAccelerationSpeed;

        Quaternion r = Quaternion.FromToRotation(root.transform.up.normalized, (rotation.x < 0 ? -1 : 1) * root.transform.right.normalized);
        root.transform.localRotation = Quaternion.Lerp(root.transform.localRotation, r * root.transform.localRotation, Mathf.Abs(rotation.x) * 180 * Time.deltaTime / 360);

        r = Quaternion.FromToRotation(root.transform.up.normalized, (rotation.y < 0 ? -1 : 1) * root.transform.forward.normalized);
        root.transform.localRotation = Quaternion.Lerp(root.transform.localRotation, r * root.transform.localRotation, Mathf.Abs(rotation.y) * 180 * Time.deltaTime / 360);

        r = Quaternion.FromToRotation(root.transform.forward.normalized, (rotation.z < 0 ? -1 : 1) * root.transform.right.normalized);
        root.transform.localRotation = Quaternion.Lerp(root.transform.localRotation, r * root.transform.localRotation, Mathf.Abs(rotation.z) * 180 * Time.deltaTime / 360);

        rotation -= (Time.deltaTime * rotation.normalized);

        if (Time.time > 0.1 + timeSinceLastDust)
        {
            GameObject nextThrustDust = null;
            if (thrustDust.Count == 0 || thrustDust.Peek().activeSelf == true)
                nextThrustDust = GameObject.Instantiate(Resources.Load("Prefabs/ThrustDust") as GameObject);
            else
                nextThrustDust = thrustDust.Dequeue();

            ThrustDustMovement dust = nextThrustDust.GetComponent<ThrustDustMovement>();
            nextThrustDust.transform.localPosition = root.transform.localPosition + -root.transform.up * 5;
            dust.velocity = world.velocity;
            dust.setTimeout(5);
            thrustDust.Enqueue(nextThrustDust);
            timeSinceLastDust = Time.time;
        }
            //transform.localPosition += velocity * Time.deltaTime;
            //TheRoot.transform.localPosition += cameraPoint.up * .1f;
    }

    private void ThirdPerson()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            lastMousePos = GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            GameObject target = transform.parent.gameObject;

            Vector3 up, right, forward;
            Vector2 mouseDelta = lastMousePos - (Vector2)GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
            mouseDelta.y = -mouseDelta.y;

            Matrix4x4 targetInv = Matrix4x4.TRS(Vector3.zero, target.transform.localRotation, target.transform.localScale).inverse;

            up = transform.localPosition + targetInv.MultiplyPoint(transform.up.normalized);
            right = transform.localPosition + targetInv.MultiplyPoint(transform.right.normalized);
            forward = transform.localPosition + targetInv.MultiplyPoint(transform.forward.normalized);


            // Move "right" and "left" relative to the camera
            up = (RotatePointAroundSphere(up, Vector3.zero, Vector3.up, mouseDelta.x * rotationSpeed));
            right = (RotatePointAroundSphere(right, Vector3.zero, Vector3.up, mouseDelta.x * rotationSpeed));
            forward = (RotatePointAroundSphere(forward, Vector3.zero, Vector3.up, mouseDelta.x * rotationSpeed));

            transform.localPosition = RotatePointAroundSphere(transform.localPosition,
                                        Vector3.zero, Vector3.up,
                                        mouseDelta.x * rotationSpeed);

            // Move "up" and "down" relative to the camera
            Vector3 relativeRight = targetInv.MultiplyPoint(transform.right);
            up = RotatePointAroundSphere(up, Vector3.zero, relativeRight, mouseDelta.y * rotationSpeed);
            right = RotatePointAroundSphere(right, Vector3.zero, relativeRight, mouseDelta.y * rotationSpeed);
            forward = RotatePointAroundSphere(forward, Vector3.zero, relativeRight, mouseDelta.y * rotationSpeed);

            transform.localPosition = RotatePointAroundSphere(transform.localPosition,
                                        Vector3.zero, relativeRight,
                                        mouseDelta.y * rotationSpeed);

            // Turn camera towards target
            transform.localRotation = Quaternion.FromToRotation(targetInv.MultiplyPoint(transform.up), up - transform.localPosition) * transform.localRotation;
            transform.localRotation = Quaternion.FromToRotation(targetInv.MultiplyPoint(transform.right), right - transform.localPosition) * transform.localRotation;
            transform.localRotation = Quaternion.FromToRotation(targetInv.MultiplyPoint(transform.forward), forward - transform.localPosition) * transform.localRotation;

            // Remove drifting from the camera rotation & position
            float range = 90;

            Vector3 removeZEuler = transform.localRotation.eulerAngles;
            if (Mathf.Abs(Vector3.Dot(transform.localPosition.normalized, Vector3.up.normalized)) < 0.97f)
            {
                if (Mathf.Abs(removeZEuler.z) < range || Mathf.Abs(removeZEuler.z) > 360 - range)
                    removeZEuler.z = 0;
                else if (Mathf.Abs(removeZEuler.z) > 180 - range && Mathf.Abs(removeZEuler.z) < 180 + range)
                    removeZEuler.z = 180;
            }

            transform.localRotation = Quaternion.Euler(removeZEuler);                       // Remove rotation drift
            transform.localPosition = transform.localPosition.normalized * distance;        // Remove translation drift
            lastMousePos = GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
        }
    }

    public static Vector3 RotatePointAroundSphere(Vector3 pointPos, Vector3 sphereCenterPos, Vector3 rotationAxis, float angleDeg)
    {
        Matrix4x4 r = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angleDeg, rotationAxis), Vector3.one);
        Matrix4x4 invP = Matrix4x4.TRS(sphereCenterPos, Quaternion.identity, Vector3.one);
        Matrix4x4 p = Matrix4x4.TRS(-sphereCenterPos, Quaternion.identity, Vector3.one);
        return (invP * r * p).MultiplyPoint(pointPos);
    }
}
