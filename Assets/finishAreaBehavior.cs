using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishAreaBehavior : MonoBehaviour {

    public string nextLevel;
    private float timer = 0;
    private bool ready = false;

    private GlobalGameData ggd;
    AudioManagerBehavior audio;

	// Use this for initialization
	void Start () {
        ggd = FindObjectOfType<GlobalGameData>();
        audio = FindObjectOfType<AudioManagerBehavior>();
	}

    public void Update()
    {
        if (ready)
        {
            timer += Time.deltaTime;
            if (timer > 3)
            {
                SceneManager.LoadScene(nextLevel);
            }
        } 
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<GunBehavior>() != null)
        {
            ready = true;
            audio.playClipHere("complete", transform.position);
            audio.stopMusic();
        }
    }
}
