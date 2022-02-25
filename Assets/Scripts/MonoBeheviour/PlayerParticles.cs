using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem coinTake;

    private void OnTriggerEnter(Collider other)
    {
        int otherTag = other.tag.GetHashCode();

        if(otherTag.Equals(Constants.Tags.COIN_TAG))
        {
            coinTake.Play();
            print("GotCoin");
        }
    }
}
