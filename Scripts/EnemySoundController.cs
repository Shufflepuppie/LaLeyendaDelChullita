using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip fuckGhosts;
   

    public void playFuckGhosts()
    {
        audioSource.PlayOneShot(fuckGhosts);
    }

   
}
