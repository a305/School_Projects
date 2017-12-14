// Nathan Pham
// CS451 Autumn 2017
// November 23rd, 2017
// This Script is used to model the manipulations done for a specific camera
//#define DEBUG_ON
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManipulation : MonoBehaviour
{
    public Camera cam;
    public Transform lookAt;
    const float zoomInterval = 1f;

    private float mMouseX = 0f; // mouse positions to track towards the user
    private float mMouseY = 0f;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(lookAt != null);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// The camera and look at positions will track towards wherever the user clicked
    /// </summary>
    /// <param name="dx">change in x from the current mouse position and the new mouse position</param>
    /// <param name="dy">change in y from the current mouse position and the new mouse position</param>
    /// <param name="kPixelToDistant">The distance to move per track (chosen by manipulator)</param>
    public void PerformCameraTracking(float dx, float dy, float kPixelToDistant)
    {
        // change x based on change in mouse position and the camera's right direction
        // change y based on the change in mouse position and the camera's  up direction                  
        Vector3 delta = (dx * kPixelToDistant * cam.transform.right)
                        + (dy * kPixelToDistant * cam.transform.up);
        cam.transform.localPosition += delta;
        lookAt.localPosition += delta;
    }

    /// <summary>
    /// This method will make the camera rotate x degrees with respect to its up orientation
    /// </summary>
    /// <param name="degree">the degrees to rotate by</param>
    public void RotateCameraAboutUp(float degree)
    {
        Quaternion up = Quaternion.AngleAxis(degree, cam.transform.up);
        RotateCameraPosition(ref up);
    }

    /// <summary>
    /// This method will make the camera rotate x degrees with respect to its right orientation
    /// </summary>
    /// <param name="degree">the degrees to rotate by </param>
    public void RotateCameraAboutSide(float degree)
    {
        Quaternion side = Quaternion.AngleAxis(degree, cam.transform.right);
        RotateCameraPosition(ref side);
    }

    /// <summary>
    /// This method computes the TRS transformation in order to rotate the camera
    /// about the look at position
    /// </summary>
    /// <param name="q">Quarternion to affect (usually a part of the camera being manipulated)</param>
    private void RotateCameraPosition(ref Quaternion q)
    {
        Matrix4x4 r = Matrix4x4.TRS(Vector3.zero, q, Vector3.one); // rotation matrix
        // inverse matrix of the look at's local position
        Matrix4x4 invP = Matrix4x4.TRS(-lookAt.localPosition, Quaternion.identity, Vector3.one);
        // Create a TRS matrix based on the rotation and inverse position matrices of the look at position 
        Matrix4x4 m = invP.inverse * r * invP;

        Vector3 newCameraPos = m.MultiplyPoint(cam.transform.localPosition);

        if (Mathf.Abs(Vector3.Dot(newCameraPos.normalized, Vector3.up)) < 0.985)
        {
            cam.transform.localPosition = newCameraPos; // commence rotation with position
            // new forward orientation of the camera
            Vector3 v = (lookAt.localPosition - cam.transform.localPosition).normalized;
            
            // new right orientation of the camera 
            Vector3 w = Vector3.Cross(v, cam.transform.up).normalized;
            
            // new up orientation of the camera
            Vector3 u = Vector3.Cross(w, v).normalized;

            //   Forward-Vector MUST BE set LAST!!: both of the following works!
            //          Right, Up, Forward
            //          Up, Right, Forward
            cam.transform.up = u;
            cam.transform.right = w;
            cam.transform.forward = v;
        }
    }

    /// <summary>
    /// moves the camera closer towards the zoom direction by the zoom distance
    /// </summary>
    /// <param name="zoomDir">1 to zoom in, -1 to zoom out</param>
    public void PerformDolly(float zoomDir)
    {
        // Determine the normal vector of the look at position and the camera
        Vector3 v = lookAt.localPosition - cam.transform.localPosition;
        // based on the zoom direction, move the camera closer or further
        // to zoom in and out, respectively
        cam.transform.localPosition += v.normalized * zoomDir * zoomInterval;

#if DEBUG_ON
        Debug.Log("Cam Position\n" + cam.transform.localPosition.ToString() );
#endif
    }

    // Getters
    public float GetMouseX() { return mMouseX; }
    public float GetMouseY() { return mMouseY; }

    // Setters
    public void SetLookAtPos(Vector3 p) { lookAt.localPosition = p; }
    public void SetMouseX(float x) { mMouseX = x; }
    public void SetMouseY(float y) { mMouseY = y; }
}
