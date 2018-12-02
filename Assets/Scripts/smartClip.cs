using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class smartClip : MonoBehaviour {

    public AudioClip[] tracks;
    public bool[] loops;

    private AudioSource src;
    private int index = 0;

	// Use this for initialization
	void Start () {
        src = GetComponent<AudioSource>();
        //print(tracks.Length);
        //print(loops.Length);
        index = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (!src.isPlaying)
        {
            if (index >= tracks.Length)
            {
                Destroy(gameObject);
                return;
            }
            src.clip = tracks[index];
            src.loop = loops[index];
            src.Play();
            index += 1;
        }
	}

    public void StopLoop()
    {
        src.loop = false;
    }
}
