﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PP : MonoBehaviour
{
    public GameObject text;
    public GameObject temp;
    bool play;
    // Start is called before the first frame update
    void Start()
    {
        text.GetComponent<Text>().text = "Pause";
        play = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeSetting()
    {
        if(play==true)
        {
            play = false;
            text.GetComponent<Text>().text = "Play";
            Debug.Log("PAUSED");
            temp.GetComponent<MainGame>().pauseGame();
        }
        else
        {
            play = true;
            text.GetComponent<Text>().text = "Pause";
            Debug.Log("PLAYING");
            temp.GetComponent<MainGame>().unpauseGame();
        }
    }
}
