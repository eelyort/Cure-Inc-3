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
        text.GetComponent<Text>().text = "Score: 0\nTime Remaining: 0";

    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<Text>().text = "Score: 0\nTime Remaining: 1";
    }
}
