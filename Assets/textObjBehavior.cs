using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textObjBehavior : MonoBehaviour {

    public string text;
    public float speed = .1f;
    public float maxX = 10;
    public float maxY = 5;
    public float spacing = 1;
    public int voice = 0;

    private int index = 0;
    private float timer = 0;
    private float posx = 0;
    private float posy = 0;
    private GlobalGameData ggd;
    private AudioManagerBehavior audio;
    private List<GameObject> printed;
    private bool done = false;


	// Use this for initialization
	void Start () {
        ggd = FindObjectOfType<GlobalGameData>();
        audio = FindObjectOfType<AudioManagerBehavior>();
        printed = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (index < text.Length && timer >= speed)
        {
            //print next char
            if (posx > maxX)
            {
                posx = 0;
                posy -= spacing;
            }
            if (text[index] != ' ')
            {
                GameObject g = ggd.getCharacter(text[index]);
                GameObject ginst = GameObject.Instantiate(g);
                ginst.transform.position = new Vector2(transform.position.x + posx, transform.position.y + posy);
                printed.Add(ginst);
            }
            timer = 0;
            index += 1;
            posx += spacing;
            audio.playVoice(voice,transform.position);
        }
        else if (index >= text.Length)
        {
            done = true;
        }
	}

    public void newString(string next)
    {
        index = 0;
        posx = 0;
        posy = 0;
        text = next;
        done = false;
        foreach (GameObject g in printed)
        {
            Destroy(g);
        }
    }

    public bool getDone()
    {
        return done;
    }

    public void finish()
    {
        for (; index < text.Length;++index )
        {
            //print next char
            if (posx > maxX)
            {
                posx = 0;
                posy -= spacing;
            }
            if (text[index] != ' ')
            {
                GameObject g = ggd.getCharacter(text[index]);
                GameObject ginst = GameObject.Instantiate(g);
                ginst.transform.position = new Vector2(transform.position.x + posx, transform.position.y + posy);
                printed.Add(ginst);
            }
            timer = 0;
            posx += spacing;
        }
        index = text.Length;
        done = true;
    }
}
