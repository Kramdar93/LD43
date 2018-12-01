using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetBehavior : MonoBehaviour {

    public float maxDist = 10f;

    private MusclesBehavior mb;
    private GunBehavior gb;
    private SpringJoint2D mjoint;

	// Use this for initialization
	void Start () {
        mb = FindObjectOfType<MusclesBehavior>();
        gb = FindObjectOfType<GunBehavior>();
        mjoint = GetComponent<SpringJoint2D>();
	}
	
	// Update is called once per frame
	//void Update () {
	//	
	//}

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, mb.transform.position) > maxDist)
        {
            mjoint.connectedBody = gb.rb2;
        }
        else
        {
            mjoint.connectedBody = mb.rb2;
        }
    }
}
