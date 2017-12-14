/// Created by Stanley Mugo and Nathan Pham on Nov 18, 2017
/// 
/// Manages selected objects.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheWorld : MonoBehaviour
{
	public enum AxisModes {
		SCALING, TRANSLATING
	}
	public static AxisModes axisMode = AxisModes.TRANSLATING;

	public int selectableLayer;
	private Selectable selectHandler = null;
    public SceneNode TheRoot = null;
    public Text healthDisplay;
    public Transform asteroidSpawningPoint;
    public HierarchyTree htree;
    public Camera mainCamera;
    public Camera secondaryCamera;
    public GameObject editorMode;
    public GameObject gameMode;
    public Text warning;
    public Text shipHealth;
    public List<StationarySentry> existingSentries;
    public CreateBoundingBox bound;
    public Vector3 velocity;
    private float health = 100f;
    public bool isPlayTime = false;
    private float timePastInstantiation = 0f;
    private float generateInterval = 1f;
    private Vector3 oldPos;
    private Quaternion oldRot;
    public List<GiantAsteroid> giantAsteroids;
    public List<Asteroid> asteroids;
    private float giantGenInterval = 15f;
    private float curgiantGen = 0f;

    private void Awake()
    {
        Update();
    }

    private void Start()
    {

    }

    public void SetPlayMode()
    {
        if (htree == null)
            return;
        List<SceneNode> cannonList = htree.FindByNameList("Cannon");
        if (cannonList != null && cannonList.Count > 0)
            foreach (SceneNode s in cannonList)
            {
                s.gameObject.GetComponent<Cannon>().active = !s.gameObject.GetComponent<Cannon>().active;
                s.gameObject.GetComponent<Cannon>().world = this;
            }

        if (htree.GetSize() < 3)
        {
            warning.text = "Must have at least 3 objects in the Hierarchy before playing.";
            return;
        }

        warning.text = "";
        isPlayTime = !isPlayTime;

        if (isPlayTime)
        {
            oldPos = TheRoot.transform.localPosition;
            oldRot = TheRoot.transform.localRotation;
            bound.DestroyCube();
            gameMode.SetActive(true);
            editorMode.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            Vector3[] sz = bound.GetBounds();
            mainCamera.transform.localPosition = TheRoot.transform.localPosition + new Vector3(0, -5 - sz[1].y / 2, 0);
            mainCamera.transform.LookAt(TheRoot.transform);
            mainCamera.transform.SetParent(TheRoot.transform, true);
            secondaryCamera.gameObject.SetActive(true);
            secondaryCamera.transform.localPosition = TheRoot.transform.localPosition + new Vector3(0, 5 + sz[1].y / 2, 0);
            secondaryCamera.transform.LookAt(TheRoot.transform);
            secondaryCamera.transform.localRotation *= Quaternion.AngleAxis(180f, TheRoot.transform.forward);
            secondaryCamera.transform.SetParent(TheRoot.transform, true);
            ForceDeselect();
        }
        else
        {
            mainCamera.transform.SetParent(null, true);
            secondaryCamera.transform.SetParent(null, true);
            gameMode.SetActive(false);
            editorMode.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            secondaryCamera.gameObject.SetActive(false);
            ResetHierarchy();
        }
    }

    private void ResetHierarchy()
    {
        TheRoot.transform.localPosition = oldPos;
        TheRoot.transform.localRotation = oldRot;
    }

    public bool IsPlayMode() { return isPlayTime; }

    private void Update()
    {
        shipHealth.text = "Space Ship Health: " + health;
        if (health <= 0)
        {
            SetPlayMode();
            warning.text = "Game over! Ship destroyed!";
            health = 100;
        }
        if (Input.GetKeyDown(KeyCode.X))
            if (giantAsteroids.Count > 0)
            {
                for (int k = 0; k < giantAsteroids.Count; k++)
                {
                    GiantAsteroid ga = giantAsteroids[k];
                    giantAsteroids.Remove(giantAsteroids[k]);
                    GameObject.Destroy(ga.gameObject);
                }
            }
        if (TheRoot != null)
        {
            Matrix4x4 i = Matrix4x4.identity;
            TheRoot.CompositeXform(ref i);
            if (isPlayTime)
            {
                TheRoot.transform.localPosition += TheRoot.transform.up * .1f;
                velocity = TheRoot.transform.up * .1f;
                timePastInstantiation += Time.deltaTime;
                curgiantGen += Time.deltaTime;

                if (curgiantGen >= giantGenInterval)
                {
                    GameObject g = Instantiate(Resources.Load("Prefabs\\GiantAsteroid")) as GameObject;
                    g.GetComponent<GiantAsteroid>().world = this;
                    // Set spawning location
                    g.transform.localPosition = new Vector3(Random.Range(asteroidSpawningPoint.transform.localPosition.x - 50f,
                        asteroidSpawningPoint.transform.localPosition.x + 50f), Random.Range(asteroidSpawningPoint.transform.localPosition.y - 50f,
                        asteroidSpawningPoint.transform.localPosition.y + 50f), Random.Range(asteroidSpawningPoint.transform.localPosition.z - 50f,
                        asteroidSpawningPoint.transform.localPosition.z + 50f));
                    giantAsteroids.Add(g.GetComponent<GiantAsteroid>());
                    // Sets direction of travel and speed of asteroid
                    curgiantGen = 0f;
                }

                if (timePastInstantiation >= generateInterval)
                {
                    GameObject g = Instantiate(Resources.Load("Prefabs\\Asteroid")) as GameObject;
                    g.GetComponent<Asteroid>().Initialize(this);
                    // Set spawning location
                    g.transform.localPosition = new Vector3(Random.Range(asteroidSpawningPoint.transform.localPosition.x - 5f,
                        asteroidSpawningPoint.transform.localPosition.x + 5f), Random.Range(asteroidSpawningPoint.transform.localPosition.y - 5f,
                        asteroidSpawningPoint.transform.localPosition.y + 5f), Random.Range(asteroidSpawningPoint.transform.localPosition.z - 5f,
                        asteroidSpawningPoint.transform.localPosition.z + 5f));

                    // Sets direction of travel and speed of asteroid
                    g.GetComponent<Asteroid>().SetTravellingDirection(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
                    g.GetComponent<Asteroid>().SetSpeed(15f);
                    asteroids.Add(g.GetComponent<Asteroid>());
                    timePastInstantiation = 0f;
                }
            }
            else
            {
                for (int k = 0; k < giantAsteroids.Count; k++)
                {
                    GiantAsteroid ga = giantAsteroids[k];
                    giantAsteroids.Remove(giantAsteroids[k]);
                    GameObject.Destroy(ga.gameObject);
                }
                for (int k = 0; k < asteroids.Count; k++)
                {
                    Asteroid ast = asteroids[k];
                    asteroids.Remove(asteroids[k]);
                    GameObject.Destroy(ast.gameObject);
                }
                foreach (StationarySentry sentry in existingSentries)
                    sentry.enabled = true;
            }
        }
    }

    // Preconditions: asteroid and collider are not null
    // Postconditions: Returns whether the asteroid collided with the ship.
    /// <summary>
    /// Returns whether the object collided with a transform
    /// </summary>
    /// <param name="tester">test object to collide</param>
    /// <param name="collider">desired object to check collision</param>
    /// <returns>true if the asteroid collides with the desired object, false if otherwise</returns>
    public bool ProcessCollision(Transform tester, Transform collider)
    {
        if (tester == null || collider == null)
            return false;
        Vector3 delta = tester.transform.position - collider.position;
        SceneNode cur = collider.GetComponent<SceneNode>();

        foreach (Transform child in collider) // Traverse the collider tree
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            //Debug.Log("Traversing: " + collider.gameObject.name);
            if (cn != null)
            {
                foreach (NodePrimitive geom in cn.PrimitiveList) // check if the node contains meshes
                {
                    AllMesh mesh = geom.GetComponent<AllMesh>();
                    bool isContactMesh = false;
                    if (mesh != null) // check collision with each vertex in the mesh
                    {
                        for (int i = 0; i < mesh.GetVertexLocations().Count; i++)
                        {
                            isContactMesh = ProcessCollision(tester, mesh.GetVertexLocations()[i]);
                            //Debug.Log(string.Format("Going through vertex {0}", i));
                            if (isContactMesh)
                            {
                                if (htree.IsLeaf(cur) && tester.GetComponent<Asteroid>() != null)
                                {
                                    //htree.Delete(ref cur);
                                    health -= 10 * Time.deltaTime;
                                }
                                return true;
                            }
                        }
                    }
                }
                return ProcessCollision(tester, cn.transform); // traverse the child transform
            }
        }

        // Hit radius of the object
        float hitRadius = Mathf.Sqrt(Mathf.Pow(collider.lossyScale.x, 2f) +
                            Mathf.Pow(collider.lossyScale.y, 2f) + Mathf.Pow(collider.lossyScale.z, 2f));
        if (hitRadius < 1)
            hitRadius = 1;
        if (delta.magnitude < tester.localScale.z && delta.magnitude < hitRadius) // condition of hit detection
        {
            if (cur != null)
            {
                if (htree.IsLeaf(cur) && tester.GetComponent<Asteroid>() != null)
                {
                    health -= 10 * Time.deltaTime;
                }
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// Deselect what ever is currently selected.
    /// </summary>
    public void ForceDeselect()
	{
		if (selectHandler != null)
		{
			selectHandler.OnForcedDeselect();
			selectHandler = null;
		}
	}

	/// <summary>
	/// Allows for the selected object to be set.
	/// Invokes the proper callback which a new value is selcted.
	/// </summary>
	/// <param name="mPos">The position that the mouse is pointing to.</param>
	/// <param name="g">The selected game object.</param>
	public void SetSelected(Vector3 mPos, ref GameObject g)
	{
		Selectable lastSelected = selectHandler;
		if (g != null)
		{
			Selectable iSelect = null;

			if (g.GetComponent<Translatable>() != null)
			{
				g.GetComponent<Translatable>().enabled =
					(TheWorld.axisMode == TheWorld.AxisModes.TRANSLATING);
			}

			if (g.GetComponent<Scalable>() != null)
			{
				g.GetComponent<Scalable>().enabled =
					(TheWorld.axisMode == TheWorld.AxisModes.SCALING);
			}

			foreach (Selectable s in g.GetComponents<Selectable>())
			{
				if (s.enabled)
				{
					iSelect = s;
					break;
				}
			}

			DeselectSelected(iSelect);

			if (iSelect != null)
			{
				if (iSelect.OnSelect(mPos, g, lastSelected))
					selectHandler = iSelect;
				else
					DeselectSelected();
			}
		} else
			DeselectSelected();

	}

	/// <summary>
	/// Allows for the selected object to be draged if it implements it.
	/// </summary>
	/// <param name="mPos">The position that the mouse is pointing to.</param>
	/// <param name="g">The selected game object.</param>
	public void DragedSelected(Vector3 mPos, ref GameObject g)
	{
		if (selectHandler != null)
			if (!selectHandler.OnDrag(mPos, g))
				DeselectSelected();
	}

	/// <summary>
	/// Checks if the selected object should be deselected when released.
	/// </summary>
	public void ReleaseSelected(Selectable toSelect = null)
	{
		if (selectHandler != null)
			if (!selectHandler.OnDeselect(toSelect))
				DeselectSelected();
	}

	/// <summary>
	/// Called when the user selects an empty region in space. Notifies
	/// the proper callback of the change in selection.
	/// </summary>
	public void DeselectSelected(Selectable toSelect = null)
	{
		if (selectHandler != null)
		{
			selectHandler.OnDeselect(toSelect);
			selectHandler = null;
		}
	}

	/// <summary>
	/// Returns whether their is shape that is currenly selected.
	/// </summary>
	/// <returns>True if there is a selected shape, else false.</returns>
	public bool HasSelected()
	{
		return selectHandler != null;
	}
}
