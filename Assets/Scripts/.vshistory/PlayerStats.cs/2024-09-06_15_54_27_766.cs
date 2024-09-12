using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : Creature
{
    
    protected override void Start()
    {
        maxHealth = 100f;
        base.Start();
    }

 
}
