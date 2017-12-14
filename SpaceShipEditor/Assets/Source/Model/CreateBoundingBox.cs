using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBoundingBox : MonoBehaviour {
	public TheWorld world;
    private bool active = false;
    private GameObject bound;

	public void Start()
	{
		gameObject.GetComponent<Button>().onClick.AddListener(() =>
		{
            if (!active)
            {
                active = true;
                Vector3[] sz = GetBounds();

                // Create, move, and resize the cube
                bound = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bound.transform.localPosition = sz[0];
                bound.transform.localScale = sz[1];
            }
            else
            {
                active = false;
                Destroy(bound);
            }
		});
	}

    public void DestroyCube()
    {
        if (bound != null)
        {
            Destroy(bound);
            active = false;
        }
    }

    public Vector3[] GetBounds()
    {
        Vector3[] sz = SceneNodeBounds.getBounds(world.TheRoot);            // Get bounds

        // Print the returned position & Size of the cube
        string s = "";
        foreach (Vector3 v in sz)
        {
            s += "(" + sz[0].x + ", " + sz[0].y + ", " + sz[0].z + ") ";
        }
        Debug.Log(s);

        // Change the position to the center position instead of the top-left-forward most point
        sz[0].x -= sz[1].x / 2;
        sz[0].y -= sz[1].y / 2;
        sz[0].z -= sz[1].z / 2;

        return sz;
    }
}
