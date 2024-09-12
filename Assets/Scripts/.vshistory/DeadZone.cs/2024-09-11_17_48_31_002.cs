using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public Creature creature;

    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
        creature.Die();

        if (other.gameObject.CompareTag ("Enemy")) 
        {
            Destroy (other.gameObject);
        }
    }


}
