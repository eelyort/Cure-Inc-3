using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    int diff = 1;

    public void setEasy()
    {
        Debug.Log("Set Easy!");
        diff = 0;
    }

    public void setCasual()
    {
        Debug.Log("Set Casual!");
        diff = 1;
    }

    public void setHard()
    {
        Debug.Log("Set Hard!");
        diff = 2;
    }

    public int getDiff()
    {
        return diff;
    }
}
