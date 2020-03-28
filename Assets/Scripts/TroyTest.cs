using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroyTest : MonoBehaviour
{
    public int FVstart = 10000;
    public int WBstart = 100;
    public int bodyCells = 50000;
    public int infectedBCstart = 100;

    public double deadVirusperWhiteBlood = 5;
    public double deadWBperdeadV;
    public double deadICperWB;
    public double infectedCellsperFV;
    public double FVperIC;
    public double chanceICbursts;
    public double spreadPerPV;

    public double whiteResistanceToInfection;

    public int breakEvenPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
