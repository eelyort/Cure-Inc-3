using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameSettings {
    public ulong freeVirusStart;
    public uint whiteBloodStart;
    public uint bodyCells;
    public uint infectBodyStart;
    public double deadVirusperWhiteBlood;
    public double deadWhiteBloodperDeadVirus;
    public double deadInfectedCellsperVirus;
    public double infectedCellsperVirus;
    public double virusesPerInfectedCell;
    public double chanceICbursts;
    public double spreadPerVirus;
    public double whiteBloodResistance;
    public int breakEvenPoint;

    public int startInfectedNodes;

    public int playerSpawnRate;

    public GameSettings(ulong FVStart, uint WBStart, uint BC, uint IBCStart, double a, double b, double c, double d, double e, double f, double g, double h, int breakEvenPoint) {
        freeVirusStart = FVStart;
        whiteBloodStart = WBStart;
        bodyCells = BC;
        infectBodyStart = IBCStart;
        deadVirusperWhiteBlood = a;
        deadWhiteBloodperDeadVirus = b;
        deadInfectedCellsperVirus = c;
        infectedCellsperVirus = d;
        virusesPerInfectedCell = e;
        chanceICbursts = f;
        spreadPerVirus = g;
        whiteBloodResistance = h;
        this.breakEvenPoint = breakEvenPoint;

        startInfectedNodes = 1;

        playerSpawnRate = 10;
    }
}