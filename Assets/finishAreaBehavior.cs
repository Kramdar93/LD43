using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishAreaBehavior : MonoBehaviour {

    public string nextLevel;

    private GlobalGameData ggd;

	// Use this for initialization
	void Start () {
        ggd = FindObjectOfType<GlobalGameData>();
	}

    public void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<GunBehavior>() != null)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
