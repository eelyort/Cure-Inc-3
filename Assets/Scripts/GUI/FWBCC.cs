using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FWBCC : MonoBehaviour
{
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        text.GetComponent<Text>().text = "Free White Blood Cell Count: 0";
    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<Text>().text = "Free White Blood Cell Count: 1";
    }
}
