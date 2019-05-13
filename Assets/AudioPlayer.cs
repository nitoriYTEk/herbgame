using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {

    AudioSource source;
    
    public AudioClip[] clips;
    public float[] volumes;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play(int index)
    {
        source.PlayOneShot(clips[index], volumes[index]);
    }

    public void PlayMusic()
    {
        source.Play();
    }
}
