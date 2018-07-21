using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class Controller : MonoBehaviour
{
	private AllMesh selected; // the mesh that will be selected in the world
	/// <summary>
	/// Checks if something has been pressed by polling the current mouse status.
	/// Called once per frame.
	/// </summary>
	public void ProcessMouseEvents()
	{
		if (EventSystem.current.IsPointerOverGameObject() && !myWorld.HasSelected()) return;

        // If you left alt on a mesh object, it will activate it
        // Code by nathan pham
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
			GameObject objAtMouse;
			Vector3 mousePos;

			if (GetObjectAtMouse(out objAtMouse, out mousePos, 1 << myWorld.selectableLayer)) {
                if (objAtMouse.GetComponent<AllMesh>() != null)
                {
                    if (selected == null)
                        selected = objAtMouse.GetComponent<AllMesh>();
                    else
                    {
                        selected.HideNormals();
                        selected = objAtMouse.GetComponent<AllMesh>();
                    }
                    selected.ShowNormals();
                }
            }
        }

		// else, the selected hides its normals and becomes null
		if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
		{
			if (!myWorld.HasSelected())
			{
				if (selected != null)
				{
					selected.HideNormals();
					selected = null;
				}
			}
		} // to here
		else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			HandleMesh();
	}

	public void ProcessKeyboardEvents()
	{
		if (Input.GetKeyDown(KeyCode.T))
			selectTRS.SetTRSMode(TheWorld.AxisModes.TRANSLATING);

		if (Input.GetKeyDown(KeyCode.S))
			selectTRS.SetTRSMode(TheWorld.AxisModes.SCALING);
	}

	private void HandleMesh()
	{
		GameObject selectedObj = null;
		Vector3 selectedPos;

		if (Input.GetMouseButtonUp(0))
		{
			// LMB released
			myWorld.ReleaseSelected();
		}
		else if (Input.GetMouseButtonDown(0))
		{
			// LMB pressed for the first time
			if (GetObjectAtMouse(out selectedObj, out selectedPos, 1 << myWorld.selectableLayer))
			{
				myWorld.SetSelected(selectedPos, ref selectedObj);
			}
			else
			{
				myWorld.DeselectSelected();
			}
		}
		else if (Input.GetMouseButton(0) && myWorld.HasSelected())
		{
			// LMB dragged
			GetObjectAtMouse(out selectedObj, out selectedPos, 1 << myWorld.selectableLayer);
			myWorld.DragedSelected(selectedPos, ref selectedObj);
		}
	}

    /// <summary>
    /// Gets the object pointed to by the cursor.
    /// </summary>
    /// <param name="gObj">The game object selected by the cursor. Null if nothing selected.</param>
    /// <param name="mousePos">The position of the mouse in 3d space. Null if nothing selected.</param>
    /// <param name="layerMask">The layer mask where each bit represents one of 32 layers.</param>
    /// <returns>True if there is an object at the cursor's position else False.</returns>
    private bool GetObjectAtMouse(out GameObject gObj, out Vector3 mousePos, int layerMask)
    {
        RaycastHit hitInfo;

        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
                                        out hitInfo, Mathf.Infinity, layerMask);

        // Find what 3d point was clicked
        if (hit)
        {
            mousePos = hitInfo.point;
            gObj = hitInfo.transform.gameObject;
        }
        else
        {
            mousePos = Vector3.zero;
            gObj = null;
        }

        return hit;
    }
}
