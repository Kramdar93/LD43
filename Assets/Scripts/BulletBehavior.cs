using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {
    public Vector2 initVel;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().AddForce(initVel, ForceMode2D.Impulse);
	}

    public void OnCollisionEnter2D(Collision2D col)
    {
        SwitchBehavior sb = col.collider.GetComponent<SwitchBehavior>();
        print(col.collider.gameObject.name);
        if (sb != null)
        {
            print("in there bois");
            sb.hitMe();
        }
        else
        {
            print("no switch on collider");
        }
        GameObject.Destroy(this.gameObject);
    }
}
