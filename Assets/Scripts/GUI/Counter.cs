using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        text.GetComponent<Text>().text = "Virus Count: 0\nWhite Blood Cell Count: 0";

    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<Text>().text = "Virus Count: 1\nWhite Blood Cell Count: 1";
    }
}
