using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip BGMclip;
    public AudioSource AudioSource;
    
    public AudioClip SwipeClip;

    public void Start()
    {
        AudioSource.clip = BGMclip;
        AudioSource.Play();
    }

    public void Swipe()
    {
        AudioSource.PlayOneShot(SwipeClip);
    }
}