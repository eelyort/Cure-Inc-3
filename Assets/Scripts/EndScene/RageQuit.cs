using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RageQuit : MonoBehaviour
{
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        bool temp = true;
        if(temp)
        {
            text.GetComponent<Text>().text = "Quit";
        }
        else
        {
            text.GetComponent<Text>().text = "Rage Quit";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
