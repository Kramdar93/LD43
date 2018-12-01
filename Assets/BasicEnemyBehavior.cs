using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : PlayerBehavior
{
    public float engagementDistance = 10f;

    private GunBehavior gb;
    private MusclesBehavior mb;

    protected Transform closest;
    private footScript leftFoot;
    private footScript rightFoot;

	// Use this for initialization
	public override void Start () {
        base.Start();
        gb = FindObjectOfType<GunBehavior>();
        mb = FindObjectOfType<MusclesBehavior>();

        leftFoot = transform.Find("leftFloor").GetComponent<footScript>();
        rightFoot = transform.Find("rightFloor").GetComponent<footScript>();
	}

    public override void preHandle()
    {
        if (Vector2.Distance(transform.position, gb.transform.position) < Vector2.Distance(transform.position, mb.transform.position))
        {
            closest = gb.transform;
        }
        else
        {
            closest = mb.transform;
        }
    }

    public override float getMovement()
    {
        bool hasLeft = leftFoot.consumeHasGround();
        bool hasRight = rightFoot.consumeHasGround();

        //print(Vector2.Distance(transform.position, closest.position) + " " + hasLeft + " " + hasRight);

        if (Vector2.Distance(transform.position, closest.position) <= engagementDistance)
        {
            if (hasLeft && closest.position.x < transform.position.x)
            {
                return -1f;
            }
            if (hasRight && closest.position.x > transform.position.x)
            {
                return 1f;
            }
        }
        return 0f;
    }

    public override Vector2 getShoot()
    {
        return Vector2.zero;
    }

    public override float getJump()
    {
        return 0;
    }

    public float getDirToPlayer(){
        if (Vector2.Distance(transform.position, closest.position) <= engagementDistance)
        {
            if (closest.position.x < transform.position.x)
            {
                return -1f;
            }
            if (closest.position.x > transform.position.x)
            {
                return 1f;
            }
        }
        return 0;
    }
}
