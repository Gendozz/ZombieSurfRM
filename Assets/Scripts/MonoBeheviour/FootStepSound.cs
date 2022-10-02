using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip footStepSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Constants.Tags.FOOTSTEP_TAG))
        {
            print("Step");
            SoundManager.Instance.Play(footStepSound, true);
        }
    }
}
