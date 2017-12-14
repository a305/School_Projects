﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour {

    protected Matrix4x4 mCombinedParentXform;
    
    public Vector3 Pivot = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;

    //public Transform AxisFrame = null;

	// Use this for initialization
	protected void Start ()
    {
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    private void InitializeSceneNode()
    {
        mCombinedParentXform = Matrix4x4.identity;
    }

    // This must be called _BEFORE_ each draw!! 
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 pivot = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);  // Pivot translation
        //Matrix4x4 invPivot = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);  // inv Pivot
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        //Debug.Log( transform.gameObject.name + " Pivot\n" + pivot.ToString());
        // Sets new reference point
        // Inverse of the pivot is calculated in the node primitive class
        mCombinedParentXform = parentXform * pivot * trs;

        // propagate to all children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            //Debug.Log("Traversing: " + transform.gameObject.name);
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }
        
        // disenminate to primitives
        foreach (NodePrimitive p in PrimitiveList)
        {
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }

        //Debug.Log(transform.gameObject.name  + " local position : " + transform.localPosition.ToString());
        //Debug.Log(transform.gameObject.name + " reference position:\n" + mCombinedParentXform.ToString());
        //Debug.Log(transform.gameObject.name + " Pivot " + Pivot.ToString());
        // Compute AxisFrame 
       // if (AxisFrame != null)
        //{
         //   AxisFrame.localPosition = mCombinedParentXform.MultiplyPoint(Pivot);
         //   AxisFrame.localRotation = transform.rotation; // axis frame matches selected node's rotation
            //Debug.Log("Axis Frame Position from " + transform.gameObject.name + " : " + mCombinedParentXform.MultiplyPoint(Pivot).ToString());
       // }
    }
}