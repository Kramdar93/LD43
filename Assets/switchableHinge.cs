using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchableHinge : MonoBehaviour {

    private Vector2 oldPos;
    private bool switched = false;

    public void activate()
    {
        SpringJoint2D target = GetComponent<SpringJoint2D>();
        if (!switched)
        {
            oldPos = target.connectedAnchor;
            target.connectedAnchor = target.anchor + (Vector2)target.gameObject.transform.position;
            switched = true;
        }
        else
        {
            target.connectedAnchor = oldPos;
            switched = false;
        }
    }
}
