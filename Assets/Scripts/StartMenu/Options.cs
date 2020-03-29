using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    // int diff;

    public void Start()
    {
        GlobalStaticVariables.difficulty = 1;
    }

    public void setEasy()
    {
        Debug.Log("Set Easy!");
        GlobalStaticVariables.difficulty = 0;
    }

    public void setCasual()
    {
        Debug.Log("Set Casual!");
        GlobalStaticVariables.difficulty = 1;
    }

    public void setHard()
    {
        Debug.Log("Set Hard!");
        GlobalStaticVariables.difficulty = 2;
    }

    public int getDiff()
    {
        return GlobalStaticVariables.difficulty;
    }
}
