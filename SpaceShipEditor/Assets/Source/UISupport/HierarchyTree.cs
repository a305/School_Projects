using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyTree : MonoBehaviour
{

    public Button buttonTemplate;
    public TheWorld theWorld;
    public MainController mainC;

    ButtonNode head = null;
    int size;

    private class ButtonNode
    {
        public ButtonControl data = null;
        public List<ButtonNode> children = null;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetSize()
    {
        return size;
    }

    public SceneNode FindByName(string name)
    {
        // Base case for if head or sn are null.
        if (head == null || name == null)
            return null;

        // Recursive call to FindByNameHelper to find requested SceneNode.
        ButtonNode b = FindByNameHelper(head, ref name);

        // Return null if SceneNode with name can't be found.
        if (b == null)
            return null;

        return b.data.GetSceneNode();
    }

    public List<SceneNode> FindByNameList(string name)
    {
        // Base case for if head or sn are null.
        if (head == null || name == null)
            return null;

        // List to return objects with requested name.
        List<SceneNode> sceneList = new List<SceneNode>();

        // Recursive call to FindByNameListHelper to find requested SceneNode.
        FindByNameListHelper(head, ref name, ref sceneList);

        // Return null if SceneNode with name can't be found.
        if (sceneList.Count == 0)
            return null;

        return sceneList;
    }

    public bool UpdateSelected(ref SceneNode sn)
    {
        // Base case for if head or sn are null.
        if (head == null || sn == null)
            return false;

        // Recursive call to FindHelper to find requested SceneNode.
        ButtonNode b = FindHelper(head, ref sn);

        // Check to see if SceneNode was found, returns false if not.
        if (b == null)
            return false;

        // Performs update operation to SceneNode's button and returns true.
        b.data.UpdateVisual();
        return true;
    }

    public bool Delete(ref SceneNode sn)
    {
        // Case for if the SceneNode being deleted is null.
        if (sn == null)
            return false;

        bool isHead = false;
        if (sn == head.data.GetSceneNode())
            isHead = true;

        // Call to recursive FindHelper function to find the requested SceneNode.
        ButtonNode b = FindHelper(head, ref sn);

        // Call to recursive FindParentHelper function to find the requested SceneNode's parent.
        ButtonNode parent = FindParentHelper(head, ref sn);

        // Case for if the requested SceneNode isn't found.
        if (b == null)
            return false;

        // If the parent isn't null, remove the ButtonNode that will be deleted.
        if (parent != null)
            parent.children.Remove(b);

        // Call to recursive helper function that actually does the removal.
        DeleteHelper(b);

        if (isHead)
            head = null;

        // Redisplay hierarchy.
        DisplayAll();
        return true;
    }

    public bool Insert(ref SceneNode sn, SceneNode parent)
    {
        // Case for if the SceneNode being inserted is null.
        if (sn == null)
            return false;

        // Base case if head is null.
        if (head == null)
        {
            // Create new button and connect to necessary elements.
            head = new ButtonNode();
            Button newb = Instantiate(buttonTemplate, transform);
            head.data = newb.GetComponent<ButtonControl>();
            head.data.mainController = mainC;
            size++;
            head.data.SetSceneNode(ref sn, size);
            head.children = new List<ButtonNode>();
            theWorld.TheRoot = head.data.GetSceneNode();
            head.data.SetSelectedObject();
            return true;
        }

        // Check to make sure there is a 
        if (parent == null)
            return false;

        // Recursive call to FindHelper to find parent SceneNode.
        ButtonNode b = FindHelper(head, ref parent);

        // Check to see if parent was found, if not, returns false.
        if (b == null)
            return false;

        // Create new button and connect to necessary elements.
        ButtonNode newNode = new ButtonNode();
        b.children.Add(newNode);
        Button newButton = Instantiate(buttonTemplate, transform);
        newNode.data = newButton.GetComponent<ButtonControl>();
        newNode.data.mainController = mainC;
        size++;
        newNode.data.SetSceneNode(ref sn, size);
        newNode.children = new List<ButtonNode>();

        DisplayAll();

        // Return true to signify successful insertion.
        return true;
    }

    public bool Move(ref SceneNode target)
    {
        // Case for if the selected object is null.
        if (target == null)
            return false;

        // Call to recursive FindHelper function to find the requested SceneNode.
        ButtonNode b = FindHelper(head, ref target);

        // Call to recursive FindParentHelper function to find the requested SceneNode's parent.
        ButtonNode p = FindParentHelper(head, ref target);

        // Case for if the selected object doesn't have a parent.
        if (p == null)
            return false;

        // Call to recursive FindHelper function to find the requested new parent.
        SceneNode pscene = p.data.GetSceneNode();
        ButtonNode newp = FindParentHelper(head, ref pscene);

        // Case for if the requested target SceneNode or the requested new parent SceneNode aren't found.
        if (b == null || newp == null)
            return false;

        // Remove the ButtonNode that will be moved from its current parent.
        p.children.Remove(b);

        // Assign the new parent to the target SceneNode.
        newp.children.Add(b);
        pscene = newp.data.GetSceneNode();
        b.data.SetSceneParent(ref pscene);

        DisplayAll();
        return true;
    }

    public bool IsLeaf(SceneNode sn)
    {
        if (head == null)
            return false;

        ButtonNode b = FindHelper(head, ref sn);

        if (b == null)
            return false;

        if (b.children.Count == 0)
            return true;

        return false;
    }

    public void DisplayAll()
    {
        if (head == null)
            return;

        int counter = 0;
        DisplayHelper(head, 0, ref counter);
    }

    ButtonNode FindHelper(ButtonNode cur, ref SceneNode sn)
    {
        // Base case if current node's SceneNode matches the requested one.
        if (cur.data.GetSceneNode() == sn)
            return cur;

        // Local variable to contain any SceneNode brought back by recursive calls
        // farther down the tree.
        ButtonNode b;
        for (int i = 0; i < cur.children.Count; i++)
        {
            // Recursive call to each child node of the current ButtonNode.
            b = FindHelper(cur.children[i], ref sn);
            if (b != null)
                return b;
        }

        // Returns null if no SceneNode was found.
        return null;
    }

    ButtonNode FindByNameHelper(ButtonNode cur, ref string name)
    {
        // Base case if current node's SceneNode matches the requested name.
        if (cur.data.GetSceneNode().name == name)
            return cur;

        ButtonNode b;
        for (int i = 0; i < cur.children.Count; i++)
        {
            // Recursive call to each child node of the current ButtonNode.
            b = FindByNameHelper(cur.children[i], ref name);
            if (b != null)
                return b;
        }

        // Returns null if no SceneNode was found.
        return null;
    }

    void FindByNameListHelper(ButtonNode cur, ref string name, ref List<SceneNode> list)
    {
        // Base case if current node's SceneNode matches the requested name.
        if (cur.data.GetSceneNode().name == name)
            list.Add(cur.data.GetSceneNode());

        for (int i = 0; i < cur.children.Count; i++)
            // Recursive call to each child node of the current ButtonNode.
            FindByNameListHelper(cur.children[i], ref name, ref list);
    }

    ButtonNode FindParentHelper(ButtonNode cur, ref SceneNode sn)
    {
        // Base case if current node doesn't have any children.
        if (cur.children.Count == 0)
            return null;

        // Local variable to contain any SceneNode brought back by recursive calls
        // farther down the tree.
        ButtonNode b;
        for (int i = 0; i < cur.children.Count; i++)
        {
            // Base case if the current node is the parent of the requested SceneNode.
            if (cur.children[i].data.GetSceneNode() == sn)
                return cur;

            // Recursive call to current ButtonNode's children
            b = FindParentHelper(cur.children[i], ref sn);
            if (b != null)
                return b;
        }

        // Returns null if no SceneNode was found.
        return null;
    }

    void DeleteHelper(ButtonNode cur)
    {
        for (int i = 0; i < cur.children.Count; i++)
            DeleteHelper(cur.children[i]);
        cur.children.Clear();
        cur.data.Delete();
        cur.children = null;
        cur.data = null;
        size--;
    }

    void DisplayHelper(ButtonNode cur, int level, ref int counter)
    {
        cur.data.transform.localPosition = new Vector3(cur.data.transform.localPosition.x,
            -14 + (-18 * counter), 0);
        cur.data.SetSpacing(level);

        counter++;

        for (int i = 0; i < cur.children.Count; i++)
            DisplayHelper(cur.children[i], level + 1, ref counter);
    }
}

