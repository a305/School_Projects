using UnityEngine;

public class SceneNodeBounds
{
	/// <summary>
	/// Returns the the position & size of a cube that fully encapsulates
	/// the provided sceenode hierarchy. This is given in the form of 2 
	/// vectors. The first vector (return[0]) is the top, left, forward most
	/// point of the cube. The second vector (return[1]) is the size of the
	/// cube. All the returned points are with respect to the space that
	/// the provided root is in.
	/// </summary>
	/// <param name="root">Root node of the hierarchy to check</param>
	/// <returns>The position and size of a cube encapsulates the given hierarchy</returns>
	public static Vector3[] getBounds(SceneNode root)
	{
		if (root == null) return null;

		Vector3 posMax = Vector3.zero;
		Vector3 posMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

		foreach (NodePrimitive p in root.GetComponentsInChildren<NodePrimitive>())
		{
			Mesh m = p.GetComponent<MeshFilter>().mesh;
			foreach (Vector3 v in m.vertices)
			{
				Vector3 pos = p.worldPos.MultiplyPoint(p.transform.localPosition + v);
				if (pos.x < posMin.x) posMin.x = pos.x;
				if (pos.y < posMin.y) posMin.y = pos.y;
				if (pos.z < posMin.z) posMin.z = pos.z;
				if (pos.x > posMax.x) posMax.x = pos.x;
				if (pos.y > posMax.y) posMax.y = pos.y;
				if (pos.z > posMax.z) posMax.z = pos.z;
			}
		}

		return new Vector3[] { posMax,
			new Vector3(posMax.x - posMin.x,
						posMax.y - posMin.y,
						posMax.z - posMin.z)
		};
	}
}