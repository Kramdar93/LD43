using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerBehavior : MonoBehaviour
{

    public smartClip smartClipTemplate;
    public AudioClip[] menuMusic, levelMusic, voices;
    public AudioClip shoot, hit, miss, jump, explode, complete, coin;

    private GlobalGameData ggd;
    private int offset = 0;

    // Use this for initialization
    void Start()
    {
        ggd = FindObjectOfType<GlobalGameData>();

        if (ggd.sceneType == "level")
        {
            CameraTargetBehavior ctb = FindObjectOfType<CameraTargetBehavior>();
            smartClip newClip = GameObject.Instantiate<smartClip>(smartClipTemplate, ctb.transform);
            newClip.tracks = levelMusic;
            newClip.loops = new bool[] { false, true };
        }
        else if (ggd.prevScene != ggd.sceneType && ggd.sceneType == "menu")
        {
            CameraTargetBehavior ctb = FindObjectOfType<CameraTargetBehavior>();
            smartClip newClip = GameObject.Instantiate<smartClip>(smartClipTemplate, ctb.transform);
            newClip.tracks = menuMusic;
            newClip.loops = new bool[] { false, true };
        }
    }

    public void playClipHere(string clipName, Vector2 position)
    {
        AudioClip selected;
        switch (clipName)
        {
            case "shoot":
                selected = shoot;
                break;
            case "hit":
                selected = hit;
                break;
            case "miss":
                selected = miss;
                break;
            case "jump":
                selected = jump;
                break;
            case "explode":
                selected = explode;
                break;
            case "complete":
                selected = complete;
                break;
            case "coin":
                selected = coin;
                break;
            default:
                return;
        }
        smartClip newClip = GameObject.Instantiate<smartClip>(smartClipTemplate);
        newClip.transform.position = position;
        newClip.tracks = new AudioClip[] { selected };
        newClip.loops = new bool[] { false };
    }

    public void playVoice(int i, Vector2 pos)
    {
        int res = 2 * i + offset;

        smartClip newClip = GameObject.Instantiate<smartClip>(smartClipTemplate);
        newClip.transform.position = pos;
        newClip.tracks = new AudioClip[] { voices[res%voices.Length] };
        newClip.loops = new bool[] { false };

        if (offset > 0)
        {
            offset = 0;
        }
        else
        {
            offset = 1;
        }
    }

}
