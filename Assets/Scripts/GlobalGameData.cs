using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameData : MonoBehaviour {

    public float gravity;
    public float accuracyLimit;
    public string sceneType;
    public string prevScene;

	// Use this for initialization
	//void Start () {
	///	
	//}
	
	// Update is called once per frame
	//void Update () {
	//	
	//}


    public GameObject getCharacter(char c)
    {
        string prefabName = "";
        if (char.IsLetter(c))
        {
            prefabName = c.ToString();
        }
        else
        {
            switch (c)
            {
                case '.':
                    prefabName = "period";
                    break;
                case ',':
                    prefabName = "comma";
                    break;
                case '"':
                    prefabName = "quote";
                    break;
                case '!':
                    prefabName = "exclam";
                    break;
                case '?':
                    prefabName = "question";
                    break;
                default:
                    return null;
            }
        }
        return (GameObject)Resources.Load("prefabs/"+prefabName, typeof(GameObject));
    }

}
