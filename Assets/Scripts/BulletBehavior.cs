using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {
    public Vector2 initVel;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().AddForce(initVel, ForceMode2D.Impulse);
	}
}
