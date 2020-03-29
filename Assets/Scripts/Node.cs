using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // all adjacent nodes
    public LinkedList<Node> adjacents = new LinkedList<Node>();

    // ints
    // virus's floating freely
    public long freeViruses;
    public int whiteBloodCount;
    public int infectedWhiteBloodCells;
    // safe body cells
    public int uninfectedBodyCells;
    public int infectedBodyCells;
    public int orignalBodyCellCount;

    // game settings, should set in main cuz difficultly levels
    // V: virus, WB: white blood cell, BC: body cell, ded: dead/killed, inf: infected, C: cells, FV: free virus
    public double dedVperWB;
    public double dedWBperdedV;
    public double dedICperWB;
    public double infCperFV;
    public double FVperIC;
    public double chanceICbursts;
    public double spreadPerFV;
    // % (0-1) of the time that a white blood cell resists a virus infection, 1 (100%) makes it so white blood cells cannot be infected
    public double whiteResistanceToInfection;
    // the number of viruses needed for killing to be at 100% rate, lower than this number and the viruses die slower, higher and they die even faster
    public int breakEvenPoint;

    // slow down the action
    const double speedThrottler = 0.1;
    // white blood cells r more likely to get found cuz they're moving around
    const double whiteLikelyHoodMod = 50;

    public Node(long freeVirusStart, int whiteBloodStart, int bodyCells, int infectBodyStart, double deadVirusperWhiteBlood, double deadWhiteBloodperDeadVirus, double deadInfectedCellsperVirus, double infectedCellsperVirus, double virusesPerInfectedCell, double chanceICbursts, double spreadPerVirus, double whiteBloodResistance, int breakEvenPoint) {
        freeViruses = freeVirusStart;
        whiteBloodCount = whiteBloodStart;
        orignalBodyCellCount = bodyCells;
        infectedBodyCells = infectBodyStart;
        uninfectedBodyCells = orignalBodyCellCount - infectedBodyCells;
        
        // settings for difficulty level
        dedVperWB = deadVirusperWhiteBlood;
        dedWBperdedV = deadWhiteBloodperDeadVirus;
        dedICperWB = deadInfectedCellsperVirus;
        infCperFV = infectedCellsperVirus;
        FVperIC = virusesPerInfectedCell;
        this.chanceICbursts = chanceICbursts;
        spreadPerFV = spreadPerVirus;
        // resistance
        whiteResistanceToInfection = whiteBloodResistance;
        // breakeven point
        this.breakEvenPoint = breakEvenPoint;
    }

    // processes on tick
    public int tick() {
        // variable to add in some randomness
        double jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;

        // white blood kills viruses, but also dies after eating a certain amount
        int dedViruses = (int)(dedVperWB * (double)whiteBloodCount * (double)freeViruses/breakEvenPoint * jiggle);
        freeViruses = (long)Mathf.Max(0, freeViruses - dedViruses);
        whiteBloodCount = Mathf.Max(0, whiteBloodCount - (int)(dedViruses * dedWBperdedV));

        // pecentage white blood cells vs body cells
        double percentWhite = (double)(whiteBloodCount + infectedWhiteBloodCells) / (double)(infectedBodyCells + uninfectedBodyCells);
        // percentages of infected vs. noninfecting
        double percentWhiteInfected = (double)(infectedWhiteBloodCells) / (double)(infectedWhiteBloodCells + whiteBloodCount);
        double percentBodyInfected = (double)(infectedBodyCells) / (double)(infectedBodyCells + uninfectedBodyCells);

        // attempt to kill infected cells
        // infected white blood cells
        jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;
        infectedWhiteBloodCells = Mathf.Max(0, infectedWhiteBloodCells - Mathf.Max(1, (int)(percentWhite * percentWhiteInfected * dedICperWB * whiteBloodCount * infectedWhiteBloodCells * jiggle * whiteLikelyHoodMod)));
        // infected body cells
        jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;
        infectedBodyCells = Mathf.Max(0, infectedBodyCells - Mathf.Max(1, (int)((1 - percentWhite) * percentBodyInfected * dedICperWB * whiteBloodCount * infectedBodyCells * jiggle)));

        // Debug.Log(infectedBodyCells);

        // free virus attempt to infect open cells
        // infect non-infected white blood cells
        jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;
        int temp = (int)(percentWhite * (1 - percentWhiteInfected) * (1 - whiteResistanceToInfection) * infCperFV * Mathf.Max(freeViruses, breakEvenPoint) * jiggle * whiteLikelyHoodMod);
        // Debug.Log(string.Format("temp: {0}, percentWhite: {1}, percentWhiteInfected: {2}, whiteRes: {3}, infCperFV: {4}", temp, percentWhite, percentWhiteInfected, whiteResistanceToInfection, infCperFV));
        infectedWhiteBloodCells += Mathf.Min(temp, whiteBloodCount);
        whiteBloodCount = Mathf.Max(0, whiteBloodCount - temp);
        // infect non-infected body cells
        jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;
        int temp2 = (int)((1 - percentWhite) * (1 - percentBodyInfected) * infCperFV * Mathf.Max(freeViruses, breakEvenPoint) * jiggle);
        infectedBodyCells += Mathf.Min(temp2, uninfectedBodyCells);
        uninfectedBodyCells = Mathf.Max(0, uninfectedBodyCells - temp2);
        // viruses that infected r no longer free in the system
        freeViruses = (long)Mathf.Max(0, freeViruses - (temp + temp2));

        // virus spread to adjacent nodes
        jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;
        int numMigrators = (int)(freeViruses * spreadPerFV * jiggle);
        if (numMigrators > 0) {
            // which adjacent nodes to infect
            bool[] toInfect = new bool[adjacents.Count];
            int numSpread = 0;
            for (int i = 0; i < adjacents.Count; i++) {
                double ran = Random.Range(0.0f, 1.0f);
                jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;
                // TODO: check threshold number
                if (ran < (freeViruses * spreadPerFV * jiggle)) {
                    toInfect[i] = true;
                    numSpread++;
                }
                else {
                    toInfect[i] = false;
                }
            }
            if (numSpread > 0) {
                // infect all nodes which should be infected
                LinkedListNode<Node> curr = adjacents.First;
                for (int i = 0; i < adjacents.Count; i++) {
                    if (toInfect[i]) {
                        curr.Value.freeViruses += numMigrators / numSpread;
                    }
                    curr = curr.Next;
                }
                freeViruses = (long)Mathf.Max(0, freeViruses - (numMigrators / numSpread) * (numMigrators / (numMigrators / numSpread)));
            }
        }

        // new viruses born from bursting infected cells
        // from white blood cells
        jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;
        int temp3 = Mathf.Min(infectedWhiteBloodCells, Mathf.Max(1, (int)(chanceICbursts * infectedWhiteBloodCells * jiggle)));
        freeViruses += (int)(temp3 * FVperIC);
        infectedWhiteBloodCells -= temp3;
        // from body cells
        jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;
        int temp4 = Mathf.Min(infectedBodyCells, Mathf.Max(1, (int)(chanceICbursts * infectedBodyCells * jiggle)));
        freeViruses += (int)(temp4 * FVperIC);
        infectedBodyCells -= temp4;

        if (freeViruses == 0 || infectedBodyCells == 0 || infectedWhiteBloodCells == 0) {
            return whiteBloodCount;
        }
        return 0;
    }
}
