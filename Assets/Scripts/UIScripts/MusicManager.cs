using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private AudioSource SFXaudioSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SwipingSFX()
    {
        SFXaudioSource.Play();
    }
    
    public void StopSFX()
    {
        SFXaudioSource.Stop();
    }
}
