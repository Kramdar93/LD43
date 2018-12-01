using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footScript : MonoBehaviour {

    private bool hasGround = false;

    public void OnTriggerStay2D(Collider2D col)
    {
        //print(col.gameObject.layer + " " + LayerMask.NameToLayer("Ground"));
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            print("y u no here");
            hasGround = true;
        }
    }

    public bool consumeHasGround()
    {
        print("returning " + hasGround);
        bool tmp = hasGround;
        hasGround = false;
        return tmp;
    }
}
