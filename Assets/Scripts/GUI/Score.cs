using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public GameObject text;
    public GameObject temp;
    // Start is called before the first frame update
    void Start()
    {
        text.GetComponent<Text>().text = "Score: " + temp.GetComponent<MainGame>().getScore() + "\nTime Remaining: " + temp.GetComponent<MainGame>().getTickCount();

    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<Text>().text = "Score: " + temp.GetComponent<MainGame>().getScore() + "\nTime Remaining: " + temp.GetComponent<MainGame>().getTickCount();
    }
}
