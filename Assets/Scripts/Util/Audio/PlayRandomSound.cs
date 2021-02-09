using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomSound : MonoBehaviour
{
    public AudioClip[] sounds;
    public bool onAwake;

    private AudioSource source;
    
    private void Start()
    {
        source = GetComponent<AudioSource>();
        
        if(onAwake)
            Play();
    }

    private void OnEnable()
    {
        if(onAwake)
        {
            if(source == null)
                    source = GetComponent<AudioSource>();
            Play();    
        }
    }
    
    public void Play()
    {
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.Play();
    }
}
