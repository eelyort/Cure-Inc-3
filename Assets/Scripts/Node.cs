using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    // all adjacent nodes
    public LinkedList<Node> adjacents = new LinkedList<Node>();

    // ints
    // virus's floating freely
    ulong freeViruses;
    uint whiteBloodCount;
    uint infectedWhiteBloodCells;
    // safe body cells
    uint uninfectedBodyCells;
    uint infectedBodyCells;
    uint orignalBodyCellCount;

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
    const double speedThrottler = 0.12;
    // white blood cells r more likely to get found cuz they're moving around
    const double whiteLikelyHoodMod = 50;

    // whether the player knows about the infection here
    public bool hidden;

    public Node(ulong freeVirusStart, uint whiteBloodStart, uint bodyCells, uint infectBodyStart, double deadVirusperWhiteBlood, double deadWhiteBloodperDeadVirus, double deadInfectedCellsperVirus, double infectedCellsperVirus, double virusesPerInfectedCell, double chanceICbursts, double spreadPerVirus, double whiteBloodResistance, int breakEvenPoint) {
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

        hidden = true;
    }

    void ranJiggle(out double jiggle) {
        jiggle = (Random.Range(0.0f, 1.9f) + 0.05) * speedThrottler;
        // jiggle = speedThrottler;
        // Debug.Log("jiggle: " + jiggle/speedThrottler);
    }

    // processes on tick
    public int tick() {
        // new code
        return tickTwo();

        // old code
        // return tickOne();
    }
    public void changeVirusCount(int x) {
        freeViruses = (ulong)((long)freeViruses + x);
    }
    private int tickTwo() {
        // update hidden
        if (hidden) {
            double percentRand = Random.Range(0.0f, 1.0f);
            if (percentRand < ((long)freeViruses / breakEvenPoint)) {
                hidden = false;
            }
        }

        // retry with all the variables remaing constant until end when results r tallied
        // tallies of how much to change each value at end of this function
        long freeVirusesTally = 0;
        long whiteBloodCountTally = 0;
        long infectedWhiteBloodCellsTally = 0;
        long uninfectedBodyCellsTally = 0;
        long infectedBodyCellsTally = 0;

        // variable to add in some randomness
        double jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;

        // new viruses born from bursting infected cells, at least 1 bursts (if there is at least one)
        // from white blood cells
        ranJiggle(out jiggle);
        // int temp3 = Mathf.Min((int)infectedWhiteBloodCells, Mathf.Max(1, (int)(chanceICbursts * infectedWhiteBloodCells * jiggle)));
        int explodedWhiteBlood = atLeastOneifOne(infectedWhiteBloodCells, chanceICbursts * infectedWhiteBloodCells * jiggle);
        freeVirusesTally += (long)(explodedWhiteBlood * FVperIC);
        infectedWhiteBloodCellsTally -= explodedWhiteBlood;
        // from body cells
        ranJiggle(out jiggle);
        int explodedBody = atLeastOneifOne(infectedBodyCells, chanceICbursts * infectedBodyCells * jiggle);
        freeVirusesTally += (long)(explodedBody * FVperIC);
        infectedBodyCellsTally -= explodedBody;
        // Debug.Log("Exploded Body: " + explodedBody + ", double: " + chanceICbursts * infectedBodyCells * jiggle);
        // Debug.Log("chance: " + chanceICbursts + ", infectedBodyCells: " + infectedBodyCells + ", jiggle: " + jiggle/speedThrottler);

        // white blood kills viruses, but also dies after eating a certain amount
        int dedViruses = atLeastOneifOne(freeViruses, (dedVperWB * (double)whiteBloodCount * ((double)freeViruses / breakEvenPoint) * jiggle));
        freeVirusesTally -= dedViruses;
        whiteBloodCountTally -= (int)(dedViruses * dedWBperdedV);

        // pecentage white blood cells vs body cells
        double percentWhite = (double)(whiteBloodCount + infectedWhiteBloodCells) / (double)(infectedBodyCells + uninfectedBodyCells);
        // percentages of infected vs. noninfecting
        double percentWhiteInfected = (double)(infectedWhiteBloodCells) / (double)(infectedWhiteBloodCells + whiteBloodCount);
        double percentBodyInfected = (double)(infectedBodyCells) / (double)(infectedBodyCells + uninfectedBodyCells);

        // attempt to kill infected cells
        // infected white blood cells, at least one dies
        ranJiggle(out jiggle);
        infectedWhiteBloodCellsTally -= atLeastOneifOne(infectedWhiteBloodCells, percentWhite * percentWhiteInfected * dedICperWB * whiteBloodCount * infectedWhiteBloodCells * jiggle * whiteLikelyHoodMod);
        // infected body cells, at least one dies
        ranJiggle(out jiggle);
        infectedBodyCellsTally -= atLeastOneifOne(infectedBodyCells, (1 - percentWhite) * percentBodyInfected * dedICperWB * whiteBloodCount * infectedBodyCells * jiggle);

        // Debug.Log(infectedBodyCells);

        // free virus attempt to infect open cells
        // infect non-infected white blood cells
        ranJiggle(out jiggle);
        int newInfectWB = atLeastOneifOne(whiteBloodCount, percentWhite * (1 - percentWhiteInfected) * (1 - whiteResistanceToInfection) * infCperFV * Mathf.Min(freeViruses, breakEvenPoint) * jiggle * whiteLikelyHoodMod);
        // Debug.Log(string.Format("temp: {0}, percentWhite: {1}, percentWhiteInfected: {2}, whiteRes: {3}, infCperFV: {4}", temp, percentWhite, percentWhiteInfected, whiteResistanceToInfection, infCperFV));
        infectedWhiteBloodCellsTally += newInfectWB;
        whiteBloodCountTally -= newInfectWB;
        // infect non-infected body cells
        ranJiggle(out jiggle);
        int newInfectBody = atLeastOneifOne(uninfectedBodyCells, (1 - percentWhite) * (1 - percentBodyInfected) * infCperFV * Mathf.Min(freeViruses, breakEvenPoint) * jiggle);
        infectedBodyCellsTally += newInfectBody;
        uninfectedBodyCellsTally -= newInfectBody;
        // Debug.Log("newInfectBody: " + newInfectBody);
        // viruses that infected r no longer free in the system
        freeVirusesTally -= (newInfectWB + newInfectBody);

        // virus spread to adjacent nodes
        ranJiggle(out jiggle);
        int numMigrators = (int)(freeViruses * spreadPerFV * jiggle);
        if (numMigrators > 0 && freeViruses > (ulong)breakEvenPoint) {
            // which adjacent nodes to infect
            bool[] toInfect = new bool[adjacents.Count];
            int numSpread = 0;
            for (int i = 0; i < adjacents.Count; i++) {
                double ran = Random.Range(0.0f, 1.0f);
                ranJiggle(out jiggle);
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
                        curr.Value.changeVirusCount((int)(numMigrators / numSpread));
                    }
                    curr = curr.Next;
                }
                freeVirusesTally -= (numMigrators / numSpread) * (numMigrators / (numMigrators / numSpread));
            }
        }

        // update values and clamp as necessary
        // free viruses, [0, 800000]
        freeViruses = (ulong)Mathf.Max(0, Mathf.Min(800000, (long)freeViruses + freeVirusesTally));
        // white blood cell count, [0, 80000]
        whiteBloodCount = (uint)Mathf.Max(0, Mathf.Min(80000, (int)whiteBloodCount + whiteBloodCountTally));
        // infected white blood cells, [0, 80000]
        infectedWhiteBloodCells = (uint)Mathf.Max(0, Mathf.Min(80000, (int)infectedWhiteBloodCells + infectedWhiteBloodCellsTally));
        // uninfected body cells, [0, inf)
        uninfectedBodyCells = (uint)Mathf.Max(0, (int)uninfectedBodyCells + uninfectedBodyCellsTally);
        // infected body cells, [0, originalBodyCells]
        infectedBodyCells = (uint)Mathf.Max(0, Mathf.Min(orignalBodyCellCount, (int)infectedBodyCells + infectedBodyCellsTally));

        // last few viruses get yeeted
        if((freeViruses + (ulong)(infectedBodyCells * FVperIC * chanceICbursts) + (ulong)(infectedWhiteBloodCells * FVperIC * chanceICbursts)) < (ulong)(whiteBloodCount * 5)) {
            freeViruses = 0;
            infectedWhiteBloodCells = 0;
            infectedBodyCells = 0;
        }

        // dieoff from overpop
        if(freeViruses > (ulong)(breakEvenPoint * 5)) {
            freeViruses = (ulong)(.9 * freeViruses);
        }

        // viruses die off quickly or spread when the host cells die
        if(uninfectedBodyCells + infectedBodyCells == 0) {
            // 60% die, 10% migrate
            LinkedListNode<Node> curr = adjacents.First;
            while(curr != null) {
                curr.Value.freeViruses += (ulong)(freeViruses * .1 * (1.0/adjacents.Count));
                curr = curr.Next;
            }
            freeViruses = (ulong)(freeViruses * .3);
        }

        if (freeViruses == 0 || infectedBodyCells == 0 || infectedWhiteBloodCells == 0) {
            return (int)whiteBloodCount;
        }
        return 0;
    }
    // so i found myself doing this multiple multiple times so its here now for easier debugging lol
    // takes a number, calcVal, and returns it as long as it is greater than 0, BUT returns 1 otherwise if there is at least one entity(val) to change
    private int atLeastOneifOne(int val, int calcVal) {
        return Mathf.Min(val, Mathf.Max(1, calcVal));
    }
    // overloads
    private int atLeastOneifOne(int val, double calcVal) {
        return atLeastOneifOne(val, (int)calcVal);
    }
    private int atLeastOneifOne(uint val, int calcVal) {
        return atLeastOneifOne((int)val, calcVal);
    }
    private int atLeastOneifOne(uint val, double calcVal) {
        return atLeastOneifOne((int)val, (int)calcVal);
    }
    private int atLeastOneifOne(ulong val, double calcVal) {
        return atLeastOneifOne((int)val, (int)calcVal);
    }

    private int tickOne() {
        // variable to add in some randomness
        double jiggle = (Random.Range(0.0f, 0.4f) + 0.8) * speedThrottler;

        // new viruses born from bursting infected cells
        // from white blood cells
        ranJiggle(out jiggle);
        uint temp3 = (uint)Mathf.Min((int)infectedWhiteBloodCells, Mathf.Max(1, (int)(chanceICbursts * infectedWhiteBloodCells * jiggle)));
        freeViruses += (ulong)(temp3 * FVperIC);
        infectedWhiteBloodCells -= temp3;
        // from body cells
        ranJiggle(out jiggle);
        uint temp4 = (uint)Mathf.Min((int)infectedBodyCells, Mathf.Max(1, (int)(chanceICbursts * infectedBodyCells * jiggle)));
        freeViruses += (ulong)(temp4 * FVperIC);
        infectedBodyCells -= temp4;

        // limit amount of viruses so no overflow
        if (!(freeViruses >= 0 && freeViruses < 50000)) {
            freeViruses = 50000;
        }

        // white blood kills viruses, but also dies after eating a certain amount
        int dedViruses = (int)Mathf.Min(freeViruses, (int)(dedVperWB * (double)whiteBloodCount * (double)freeViruses / breakEvenPoint * jiggle));
        Debug.Log("dedViruses: " + dedViruses);
        freeViruses -= (uint)dedViruses;
        whiteBloodCount = (uint)Mathf.Max(0, whiteBloodCount - (uint)(dedViruses * dedWBperdedV));

        // pecentage white blood cells vs body cells
        double percentWhite = (double)(whiteBloodCount + infectedWhiteBloodCells) / (double)(infectedBodyCells + uninfectedBodyCells);
        // percentages of infected vs. noninfecting
        double percentWhiteInfected = (double)(infectedWhiteBloodCells) / (double)(infectedWhiteBloodCells + whiteBloodCount);
        double percentBodyInfected = (double)(infectedBodyCells) / (double)(infectedBodyCells + uninfectedBodyCells);

        // attempt to kill infected cells
        // infected white blood cells
        ranJiggle(out jiggle);
        infectedWhiteBloodCells = (uint)Mathf.Max(0, infectedWhiteBloodCells - Mathf.Max(1, (int)(percentWhite * percentWhiteInfected * dedICperWB * whiteBloodCount * infectedWhiteBloodCells * jiggle * whiteLikelyHoodMod)));
        // infected body cells
        ranJiggle(out jiggle);
        infectedBodyCells = (uint)Mathf.Max(0, infectedBodyCells - Mathf.Max(1, (int)((1 - percentWhite) * percentBodyInfected * dedICperWB * whiteBloodCount * infectedBodyCells * jiggle)));

        // Debug.Log(infectedBodyCells);

        // free virus attempt to infect open cells
        // infect non-infected white blood cells
        ranJiggle(out jiggle);
        int temp = (int)(percentWhite * (1 - percentWhiteInfected) * (1 - whiteResistanceToInfection) * infCperFV * Mathf.Min(freeViruses, breakEvenPoint) * jiggle * whiteLikelyHoodMod);
        // Debug.Log(string.Format("temp: {0}, percentWhite: {1}, percentWhiteInfected: {2}, whiteRes: {3}, infCperFV: {4}", temp, percentWhite, percentWhiteInfected, whiteResistanceToInfection, infCperFV));
        infectedWhiteBloodCells += (uint)Mathf.Min(temp, whiteBloodCount);
        whiteBloodCount = (uint)Mathf.Max(0, whiteBloodCount - temp);
        // infect non-infected body cells
        ranJiggle(out jiggle);
        int temp2 = (int)((1 - percentWhite) * (1 - percentBodyInfected) * infCperFV * Mathf.Min(freeViruses, breakEvenPoint) * jiggle);
        infectedBodyCells += (uint)Mathf.Min(temp2, uninfectedBodyCells);
        uninfectedBodyCells = (uint)Mathf.Max(0, uninfectedBodyCells - temp2);
        // viruses that infected r no longer free in the system
        freeViruses = (ulong)Mathf.Max(0, (long)freeViruses - (temp + temp2));

        // virus spread to adjacent nodes
        ranJiggle(out jiggle);
        int numMigrators = (int)(freeViruses * spreadPerFV * jiggle);
        if (numMigrators > 0) {
            // which adjacent nodes to infect
            bool[] toInfect = new bool[adjacents.Count];
            int numSpread = 0;
            for (int i = 0; i < adjacents.Count; i++) {
                double ran = Random.Range(0.0f, 1.0f);
                ranJiggle(out jiggle);
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
                        curr.Value.freeViruses += (ulong)(numMigrators / numSpread);
                    }
                    curr = curr.Next;
                }
                freeViruses = (ulong)Mathf.Max(0, freeViruses - (ulong)((numMigrators / numSpread) * (numMigrators / (numMigrators / numSpread))));
            }
        }

        if (freeViruses == 0 || infectedBodyCells == 0 || infectedWhiteBloodCells == 0) {
            return (int)whiteBloodCount;
        }
        return 0;
    }

    // get functions
    public long getFreeViruses() {
        if (hidden) {
            return 0;
        }
        return (long)freeViruses;
    }
    public int getWhiteBloodCount() {
        return (int)whiteBloodCount;
    }
    public int getInfectedWhiteBloodCells() {
        if (hidden) {
            return 0;
        }
        return (int)infectedWhiteBloodCells;
    }
    public int getUninfectedBodyCells() {
        if (hidden) {
            return (int)orignalBodyCellCount;
        }
        return (int)uninfectedBodyCells;
    }
    public int getInfectedBodyCells() {
        if (hidden) {
            return 0;
        }
        return (int)infectedBodyCells;
    }
    public int getOriginalCellCount() {
        return (int)orignalBodyCellCount;
    }
}
