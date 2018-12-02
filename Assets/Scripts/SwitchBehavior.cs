using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour {

    public GameObject[] connections;

    public void hitMe()
    {
        //print("boom goes my d");
        foreach(GameObject g in connections){
            if (g.GetComponent<switchableHinge>() != null)
            {
                g.GetComponent<switchableHinge>().activate();
            }
            else
            {
                g.SetActive(!g.activeSelf); //default to disabling the component...
            }
        }
    }
}
