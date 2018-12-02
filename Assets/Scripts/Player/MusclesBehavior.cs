using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusclesBehavior : PlayerBehavior
{
    private GunBehavior gun;
    private Transform grabPoint;
    private bool gunClose = false;
    private bool justPicked = false;
    private footScript leftFoot;
    private footScript rightFoot;
    private footScript jumpLeft;
    private footScript jumpRight;

    private bool jumpableLeft = false;
    private bool jumpableRight = false;
    private bool hasLeft = false;
    private bool hasRight = false;
    private bool justJumped = false;

    public override void Start()
    {
        base.Start();
        gun = FindObjectOfType<GunBehavior>();
        grabPoint = transform.Find("grabPoint");
        leftFoot = transform.Find("leftFloor").GetComponent<footScript>();
        rightFoot = transform.Find("rightFloor").GetComponent<footScript>();
        jumpLeft = transform.Find("jumpLeft").GetComponent<footScript>();
        jumpRight = transform.Find("jumpRight").GetComponent<footScript>();
    }

    public override float getMovement()
    {
        if (gun.isHeld)
        {
            return base.getMovement(); //prevent oscillation when carried.
        }
        if (Mathf.Sign(gun.transform.position.x - transform.position.x) < 0 && (hasLeft || jumpableLeft))
        {
            return -1f;
        }
        if (Mathf.Sign(gun.transform.position.x - transform.position.x) > 0 && (hasRight || jumpableRight))
        {
            return 1f;
        }
        return 0f;
    }

    public override float getJump()
    {
        float input = base.getJump();
        if (gun.isHeld)
        {
            return input;
        }
        else if(!justJumped)
        {
            float playerDir = gun.transform.position.x - transform.position.x;
            //print(playerDir + " " + jumpableLeft + " " + jumpableRight);
            if (!hasLeft && playerDir < 0 && jumpableLeft)
            {
                justJumped = true;
                return 1;
            }
            if (!hasLeft && playerDir > 0 && jumpableRight)
            {
                justJumped = true;
                return 1;
            }
            return 0f;
        }
        else if (justJumped)
        {
            justJumped = false;
        }
        return 0;
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.GetComponent<GunBehavior>() != null)
        {
            gunClose = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<GunBehavior>() != null)
        {
            print("releasing controll");
            gunClose = false;
        }
    }

    public override void preHandle()
    {
        base.preHandle();
        jumpableLeft = jumpLeft.consumeHasGround();
        jumpableRight = jumpRight.consumeHasGround();
        hasRight = rightFoot.consumeHasGround();
        hasLeft = leftFoot.consumeHasGround();
    }

    public override void postHandle()
    {
        if (gunClose && !justPicked && Input.GetAxisRaw("Vertical") > ggd.accuracyLimit && !gun.isHeld)
        {
            gun.isHeld = true;
            justPicked = true;
        }
        else if (gun.isHeld && Input.GetAxisRaw("Vertical") < -ggd.accuracyLimit)
        {
            gun.isHeld = false;
            gun.rb2.velocity = new Vector2(gun.rb2.velocity.x, gun.rb2.velocity.y - 1);
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) < ggd.accuracyLimit)
        {
            justPicked = false;
        }
        else if (gun.isHeld && !justPicked 
            && Input.GetAxisRaw("Vertical") > ggd.accuracyLimit 
            && !isOnGround())
        {
            if(gun.rb2.velocity.y < 0){
                gun.rb2.velocity = new Vector2(gun.rb2.velocity.x, 0);
            }
            gun.isHeld = false;
            if (facingLeft)
            {
                gun.remoteJump((Vector2.up + Vector2.left).normalized);

            }
            else
            {
                gun.remoteJump((Vector2.up + Vector2.right).normalized);
            }
            justPicked = true;
        }

        if (gun.isHeld)
        {
            gun.transform.position = grabPoint.position;
            gun.rb2.velocity = rb2.velocity;
        }
    }
}
