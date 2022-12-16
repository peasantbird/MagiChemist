 using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource BGM;
    public AudioClip[] musicArray;

    private AudioClip RandomTrack()
    {
        return musicArray[Random.Range(0, musicArray.Length)];
    }
    private void Start()
    {
        BGM.PlayOneShot(musicArray[0]);
    }
    private void Update()
    {
        if (BGM.isPlaying == false) 
        {
            BGM.PlayOneShot(RandomTrack(), 0.5f);
        }
    }
}
