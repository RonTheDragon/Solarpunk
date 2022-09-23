using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Plant")
        {
            other.GetComponent<Plant>().GetWatered();
        }
    }
}
