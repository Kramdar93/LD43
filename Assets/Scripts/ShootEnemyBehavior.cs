using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyBehavior : BasicEnemyBehavior {

    private bool aiJustShot = false;

    public override Vector2 getShoot()
    {
        if (!aiJustShot && Vector2.Distance(transform.position, closest.position) <= engagementDistance)
        {
            aiJustShot = true;
            return (closest.position - shootyPoint.position).normalized;
        }
        aiJustShot = false; //blip it real good
        return Vector2.zero;
    }
}
