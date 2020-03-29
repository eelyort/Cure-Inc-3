using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroyTest : MonoBehaviour
{
    public uint FVstart = 10000;
    public uint WBstart = 100;
    public uint bodyCells = 50000;
    public uint infectedBCstart = 100;

    public double deadVirusperWhiteBlood = 5;
    public double deadWBperdeadV = 1.0/50.0;
    public double deadICperWB = 1;
    public double infectedCellsperFV = 0.01;
    public double FVperIC = 50;
    public double chanceICbursts = 0.1;
    public double spreadPerPV = 0.01;

    public double whiteResistanceToInfection = 0.8;

    public int breakEvenPoint = 10000;

    public uint numWBperTick = 5;

    public int numTicks = 100;

    // Start is called before the first frame update
    void Start()
    {
        Node hi = new Node(FVstart, WBstart, bodyCells, infectedBCstart, deadVirusperWhiteBlood, deadWBperdeadV, deadICperWB, infectedCellsperFV, FVperIC, chanceICbursts, spreadPerPV, whiteResistanceToInfection, breakEvenPoint);

        Node adjacent = new Node(0, 0, 0, 0, deadVirusperWhiteBlood, deadWBperdeadV, deadICperWB, infectedCellsperFV, FVperIC, chanceICbursts, spreadPerPV, whiteResistanceToInfection, breakEvenPoint);
        hi.adjacents.AddLast(adjacent);

        Debug.Log("  Time to die");
        string format = "  {0,-2} | {1,-16} | {2,-16} | {3,-16} | {4,-16} | {5,-16} | {6,-16}";
        Debug.Log(string.Format(format, "i", "FV", "WB", "iWB", "uiBC", "iBC", "adjacent"));

        for (int i = 0; i < numTicks; i++) {
            Debug.Log(string.Format(format, i, hi.freeViruses, hi.whiteBloodCount, hi.infectedWhiteBloodCells, hi.uninfectedBodyCells, hi.infectedBodyCells, adjacent.freeViruses));
            hi.whiteBloodCount += numWBperTick;
            hi.tick();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
