using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // all adjacent nodes
    LinkedList<Node> adjacents = new LinkedList<Node>();

    // ints
    // virus's floating freely
    int freeViruses;
    int whiteBloodCount;
    // safe body cells
    int uninfectedBodyCells;
    int infectedBodyCells;
    int orignalBodyCellCount;
    int infectedWhiteBloodCells;

    // game settings, should set in main cuz difficultly levels
    // V: virus, WB: white blood cell, BC: body cell, ded: dead/killed, inf: infected, C: cells, FV: free virus
    double dedVperWB;
    double dedWBperdedV;
    double dedICperWB;
    double infCperFV;
    double FVperIC;
    double spreadPerFV;
    // % (0-1) of the time that a white blood cell resists a virus infection, 1 (100%) makes it so white blood cells cannot be infected
    double whiteResistanceToInfection;
    // the number of viruses needed for killing to be at 100% rate, lower than this number and the viruses die slower, higher and they die even faster
    int breakEvenPoint;

    public Node(int freeVirusStart, int whiteBloodStart, int bodyCells, int infectBodyStart, int infectWhiteBloodStart, double deadVirusperWhiteBlood, double deadWhiteBloodperDeadVirus, double deadInfectedCellsperVirus, double infectedCellsperVirus, double virusesPerInfectedCell, double spreadPerVirus, double whiteBloodResistance, int breakEvenPoint) {
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
        spreadPerFV = spreadPerVirus;
        // resistance
        whiteResistanceToInfection = whiteBloodResistance;
        // breakeven point
        this.breakEvenPoint = breakEvenPoint;
    }

    // processes on tick
    void tick() {
        // variable to add in some randomness
        double jiggle = Random.Range(0.0f, 0.4f) + 0.8;

        // white blood kills viruses, but also dies after eating a certain amount
        int dedViruses = (int)(dedVperWB * (double)whiteBloodCount * (double)freeViruses/breakEvenPoint * jiggle);
        freeViruses -= dedViruses;
        whiteBloodCount -= (int)(dedViruses * dedWBperdedV);

        // pecentage white blood cells vs body cells
        double percentWhite = (double)(whiteBloodCount + infectedWhiteBloodCells) / (double)(infectedBodyCells + uninfectedBodyCells);
        // percentages of infected vs. noninfecting
        double percentWhiteInfected = (double)(infectedWhiteBloodCells) / (double)(infectedWhiteBloodCells + whiteBloodCount);
        double percentBodyInfected = (double)(infectedBodyCells) / (double)(infectedBodyCells + uninfectedBodyCells);

        // attempt to kill infected cells
        // infected white blood cells
        jiggle = Random.Range(0.0f, 0.4f) + 0.8;
        infectedWhiteBloodCells -= (int)(percentWhite * percentWhiteInfected * dedICperWB * whiteBloodCount * jiggle);
        // infected body cells
        jiggle = Random.Range(0.0f, 0.4f) + 0.8;
        infectedBodyCells -= (int)((1 - percentWhite) * percentBodyInfected * dedICperWB * whiteBloodCount * jiggle);

        // free virus attempt to infect open cells
        // infect non-infected white blood cells
        jiggle = Random.Range(0.0f, 0.4f) + 0.8;
        int temp = (int)(percentWhite * (1 - percentWhiteInfected) * (1 - whiteResistanceToInfection) * infCperFV * freeViruses * jiggle);
        infectedWhiteBloodCells += temp;
        // infect non-infected body cells
        jiggle = Random.Range(0.0f, 0.4f) + 0.8;
        int temp2 = (int)((1 - percentWhite) * (1 - percentBodyInfected) * infCperFV * freeViruses * jiggle);
        infectedBodyCells += temp2;
        // viruses that infected r no longer free in the system
        freeViruses -= temp + temp2;

        // virus spread to adjacent nodes
        jiggle = Random.Range(0.0f, 0.4f) + 0.8;
        int numMigrators = (int)(freeViruses * spreadPerFV * jiggle);
        // which adjacent nodes to infect
        bool[] toInfect = new bool[adjacents.Count];
        int numSpread = 0;
        for(int i = 0; i < adjacents.Count; i++) {
            double ran = Random.Range(0.0f, 1.0f);
            jiggle = Random.Range(0.0f, 0.4f) + 0.8;
            // TODO: check threshold number
            if (ran < (freeViruses * spreadPerFV * jiggle)) {
                toInfect[i] = true;
                numSpread++;
            }
            else {
                toInfect[i] = false;
            }
        }
        // infect all nodes which should be infected
        LinkedListNode<Node> curr = adjacents.First;
        for(int i = 0; i < adjacents.Count; i++) {
            if (toInfect[i]) {
                curr.Value.freeViruses += numMigrators / numSpread;
            }
            curr = curr.Next;
        }
        freeViruses -= (numMigrators / numSpread) * (numMigrators / (numMigrators/numSpread));

        // new viruses born from infected cells
        freeViruses += (int)(FVperIC * (infectedWhiteBloodCells + infectedBodyCells));
    }
}
