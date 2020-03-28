using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextStuff : MonoBehaviour
{
    //public Text test;
    public GameObject test2;

    public void Start()
    {
        //test = gameObject.GetComponent<Text>();
        //test.text = "uwu";
    }

    public void Update()
    {
        
    }

    public void testText()
    {
        //test.text = "Hewwo";
        test2.GetComponent<Text>().text = "Hewwo";
    }

}
