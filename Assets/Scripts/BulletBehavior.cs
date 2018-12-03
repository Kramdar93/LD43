using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {
    public bool destroyOnHit = true;
    public bool playSound = true;
    public Vector2 initVel;

    private AudioManagerBehavior audioMan;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().AddForce(initVel, ForceMode2D.Impulse);
        audioMan = FindObjectOfType<AudioManagerBehavior>();
	}

    public void OnCollisionEnter2D(Collision2D col)
    {
        SwitchBehavior sb = col.collider.GetComponent<SwitchBehavior>();
        PlayerBehavior pb = col.collider.GetComponent<PlayerBehavior>();
        //print(col.collider.gameObject.name);
        if (sb != null)
        {
            audioMan.playClipHere("hit", transform.position);
            //print("in there bois");
            sb.hitMe();
        }
        else if (pb != null)
        {
            pb.onDeath();
        }
        else if (playSound)
        {
            audioMan.playClipHere("miss", transform.position);
            //print("no switch on collider");
        }
        if (destroyOnHit)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
