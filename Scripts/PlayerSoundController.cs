using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoSaltar;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoInicio;
    public AudioClip sonidoKira;
    public AudioClip sonidoDanio;

    public void playSaltar()
    {
        audioSource.PlayOneShot(sonidoSaltar);
    }

    public void playMuerte()
    {
        audioSource.PlayOneShot(sonidoMuerte);
    }

    public void playInicio()
    {
        audioSource.PlayOneShot(sonidoInicio);
    }

    public void playKira()
    {
        audioSource.PlayOneShot(sonidoKira);
    }

    public void playDanio()
    {
        audioSource.PlayOneShot(sonidoDanio);
    }
}
