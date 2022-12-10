 using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource BGM;
    public AudioClip[] musicArray;

    AudioClip RandomTrack()
    {
        return musicArray[Random.Range(0, musicArray.Length)];
    }
    void Start()
    {
        BGM.PlayOneShot(RandomTrack());
    }
    void Update()
    {
        if (BGM.isPlaying == false) 
        {
            BGM.PlayOneShot(RandomTrack());
        }
    }
}
