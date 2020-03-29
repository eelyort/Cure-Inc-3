using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public GameObject text;
    public GameObject temp;
    // Start is called before the first frame update
    void Start()
    {
        long totalViral = temp.GetComponent<MainGame>().getFreeViruses() + temp.GetComponent<MainGame>().getInfectedWhiteBloodCells() + temp.GetComponent<MainGame>().getInfectedBodyCells();
        text.GetComponent<Text>().text = "Virus Count: "+ totalViral +"\nWhite Blood Cell Count: " + temp.GetComponent<MainGame>().getWhiteBloodCount();
    }

    // Update is called once per frame
    void Update()
    {
        long totalViral = temp.GetComponent<MainGame>().getFreeViruses() + temp.GetComponent<MainGame>().getInfectedWhiteBloodCells() + temp.GetComponent<MainGame>().getInfectedBodyCells();
        text.GetComponent<Text>().text = "Virus Count: " + totalViral + "\nWhite Blood Cell Count: " + temp.GetComponent<MainGame>().getWhiteBloodCount();
    }
}
