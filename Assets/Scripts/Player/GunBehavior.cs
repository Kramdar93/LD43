using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : PlayerBehavior
{
    public bool isHeld = false;

    private MusclesBehavior muscle;

    public override void Start()
    {
        base.Start();
        muscle = FindObjectOfType<MusclesBehavior>();
        shootyPoint = transform.Find("shootyPoint");
    }

    public override float getJump()
    {
        if (isHeld)
        {
            return 0f;
        }
        return base.getJump();
    }

    public override float getMovement()
    {
        float parentRes = base.getMovement();
        if (isHeld)
        {
            if (parentRes < -ggd.accuracyLimit)
            {
                facingLeft = true;
            }
            else if (parentRes > ggd.accuracyLimit)
            {
                facingLeft = false;
            }
            return 0f;
        }
        return parentRes;
    }

}
