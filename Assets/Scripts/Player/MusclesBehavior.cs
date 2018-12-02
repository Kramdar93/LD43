using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusclesBehavior : PlayerBehavior
{
    private GunBehavior gun;
    private Transform grabPoint;
    private bool gunClose = false;
    private bool justPicked = false;

    public override void Start()
    {
        base.Start();
        gun = FindObjectOfType<GunBehavior>();
        grabPoint = transform.Find("grabPoint");
    }

    public override float getMovement()
    {
        if (gun.isHeld)
        {
            return base.getMovement(); //prevent oscillation when carried.
        }
        float res = base.getMovement();
        if (Mathf.Sign(gun.transform.position.x - transform.position.x) != Mathf.Sign(res))
        {
            return 0f;
        }
        return res;
    }

    public override float getJump()
    {
        float input = base.getJump();
        if (gunClose)
        {
            return input;
        }
        return 0f;
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
