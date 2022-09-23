using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public enum CollisionEffects { Water,Suck}
    public CollisionEffects effect = CollisionEffects.Water;
    private void OnParticleCollision(GameObject other)
    {
        if (effect == CollisionEffects.Water)
        {
            if (other.tag == "Plant")
            {
                other.GetComponent<Plant>().GetWatered();
            }
        }
        else if (effect == CollisionEffects.Suck)
        {
            if (other.tag == "Enemy")
            {
                other.transform.parent.GetComponent<GasCloud>().Sucked();
            }

        }
    }
}
