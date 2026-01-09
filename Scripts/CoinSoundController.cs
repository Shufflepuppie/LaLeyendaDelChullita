using UnityEngine;

public class CoinSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip Atun;

    public void playAtun()
    {
        audioSource.PlayOneShot(Atun);
    }
}
