using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemyBehavior : BasicEnemyBehavior
{
    private footScript jumpLeft;
    private footScript jumpRight;

    private bool jumpableLeft = false;
    private bool jumpableRight = false;

    private bool justJumped = false;

	// Use this for initialization
	public override void Start () {
        base.Start();
        jumpLeft = transform.Find("jumpLeft").GetComponent<footScript>();
        jumpRight = transform.Find("jumpRight").GetComponent<footScript>();
	}

    public override void preHandle()
    {
        base.preHandle();
        jumpableLeft = jumpLeft.consumeHasGround();
        jumpableRight = jumpRight.consumeHasGround();
    }

    public override float getMovement()
    {
        float parentMove = base.getMovement();
        float playerDir = getDirToPlayer();
        if (playerDir > 0 && jumpableRight)
        {
            return 1;
        }
        else if (playerDir < 0 && jumpableLeft)
        {
            return -1;
        }
        return parentMove;
    }

    public override float getJump()
    {
        float playerDir = getDirToPlayer();
        //print(playerDir + " " + jumpableLeft + " " + jumpableRight);
        if (!hasLeft && playerDir < 0 && jumpableLeft && !justJumped)
        {
            justJumped = true;
            return 1;
        }
        if (!hasRight && playerDir > 0 && jumpableRight && !justJumped)
        {
            justJumped = true;
            return 1;
        }
        if (justJumped)
        {
            justJumped = false;
        }
        return 0;
    }
}
