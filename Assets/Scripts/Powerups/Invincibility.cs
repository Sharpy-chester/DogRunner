using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : Powerup
{

    public override void Effect()
    {
        player = FindObjectOfType<PlayerController>();
        player.invin = true;
    }
}
