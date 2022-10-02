using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
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

    private Player player;

    // AudioClips
    [SerializeField]
    private AudioClip coinTakeSound;

    [SerializeField]
    private AudioClip hitSound;


    private void Start()
    {
        player = GetComponent<Player>();
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
            SoundManager.Instance.Play(coinTakeSound, true);
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
            player.HandleSideHit();
            return;
        }

        if(otherTag == Constants.Tags.DEATH_TAG)
        {
            #region To prevent stucking ragdoll in obstacle colliders
            //Collider[] obstacleColliders = other.gameObject.GetComponentsInParent<Collider>();
            //foreach (Collider collider in obstacleColliders)
            //{
            //    collider.enabled = false;
            //}
            #endregion

            if (player.AreYouAlive())
            {
                death.Play();
                OnDeath?.Invoke();
                SoundManager.Instance.Play(hitSound, true);
                player.KillPlayer(); 
            }
        }        
    }
}
