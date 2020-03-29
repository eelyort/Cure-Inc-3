using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoss : MonoBehaviour
{
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        string closer = "\nFinal Score: 0"; //Add score
        //Replace with boolean method to find if its a win or loss
        bool temp = true;
        if(temp)
        {
            text.GetComponent<Text>().text = "Congrats! You won!" + closer;
        }
        else
        {
            text.GetComponent<Text>().text = "GAME OVER" + closer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
