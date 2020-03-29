using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoss : MonoBehaviour
{
    public GameObject text;
    public GameObject temp;
    // Start is called before the first frame update
    void Start()
    {
        string closer = "\nFinal Score: "; //Add score
        closer += temp.GetComponent<MainGame>().getScore();
        //Replace with boolean method to find if its a win or loss
        bool tempbool = true;
        if(tempbool)
        {
            text.GetComponent<Text>().text = "Congrats! You won!" + closer;
        }
        else
        {
            text.GetComponent<Text>().text = "GAME OVER" + closer;
        }
    }
}
