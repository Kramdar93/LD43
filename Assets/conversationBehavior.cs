using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class conversationBehavior : MonoBehaviour {

    public textObjBehavior textA, textB;
    public GameObject picA, picB;
    public Sprite[] pics;
    public string[] convo1;
    public string[] convo2;
    public int[] nextSpeaker;
    private int c1ind = 0;
    private int c2ind = 0;
    public string nextLevel;


    private textObjBehavior nextBox;
    private textObjBehavior prevBox;
    private GlobalGameData ggd;
    private bool isReady = true;
    private int nextSpeakerIndex = 0;

    private bool pressed = false;

	// Use this for initialization
	void Start () {
        ggd = FindObjectOfType<GlobalGameData>();
        if (nextSpeaker[0] == 0)
        {
            nextBox = textA;
        }
        else
        {
            nextBox = textB;
        }
        prevBox = null;
        picA.SetActive(false);
        picB.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!pressed && (Input.GetAxisRaw("Submit") > ggd.accuracyLimit || Input.GetAxisRaw("Fire") > ggd.accuracyLimit))
        {
            pressed = true;
            if(!nextBox.getDone())
            {
                nextBox.finish();
            }
            else if (nextSpeakerIndex >= nextSpeaker.Length && prevBox.getDone())
            {
                if (nextLevel == "exitGame")
                {
                    Application.Quit();
                    return;
                }
                SceneManager.LoadScene(nextLevel);
            }
            else
            {
                prevBox = nextBox;
                prevBox.newString("");
                picA.SetActive(false);
                picB.SetActive(false);
                if (nextSpeaker[nextSpeakerIndex] == 0)
                {
                    picA.SetActive(true);
                    nextBox = textA;
                    nextBox.newString(convo1[c1ind]);
                    c1ind += 1;
                }
                else if (nextSpeaker[nextSpeakerIndex] == 1)
                {
                    picB.SetActive(true);
                    nextBox = textB;
                    nextBox.newString(convo2[c2ind]);
                    c2ind += 1;
                }
                nextSpeakerIndex += 1;
            }
        }
        else if (pressed && (Input.GetAxisRaw("Submit") < ggd.accuracyLimit && Input.GetAxisRaw("Fire") < ggd.accuracyLimit))
        {
            pressed = false;
        }
	}
}
