using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerCollisionHandler : MonoBehaviour
{
    public UnityEvent OnCoinCollected;

    public UnityEvent OnSideHit;

    public UnityEvent OnDeath;
    
    [SerializeField]
    private ParticleSystem coinTake;

    [SerializeField]
    private ParticleSystem sideHit;

    [SerializeField]
    private ParticleSystem death;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(other.gameObject.tag))
        {
            return;
        }
        
        int otherTag = other.gameObject.tag.GetHashCode();

        if(otherTag == Constants.Tags.COIN_TAG)
        {
            coinTake.Play();
            OnCoinCollected?.Invoke();
            other.gameObject.SetActive(false);
            return;
        }

        if(otherTag == Constants.Tags.TRAP_TAG)
        {
            other.GetComponent<Trap>().ActivateTrap();
            return;
        }


        if(otherTag == Constants.Tags.SIDEHIT_TAG)
        {            
            sideHit.Play();
            OnSideHit?.Invoke();
            playerMovement.HandleSideHit();
            return;
        }

        if(otherTag == Constants.Tags.DEATH_TAG)
        {            
            death.Play();
            OnDeath?.Invoke();
            playerMovement.KillPlayer();
        }        
    }
}
