﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
	//Node variables
	LinkedList<Node> nodeList = new LinkedList<Node>();
	LinkedListNode<Node> firstNode;
	LinkedListNode<Node> currentNode;
	
	bool paused = false;

    //Settings settings;
	
	ulong freeVirusStart; 
	uint whiteBloodStart; 
	uint bodyCells;
	uint infectBodyStart;
	double deadVirusperWhiteBlood;
	double deadWhiteBloodperDeadVirus;
	double deadInfectedCellsperVirus;
	double infectedCellsperVirus;
	double virusesPerInfectedCell;
	double chanceICbursts;
	double spreadPerVirus;
	double whiteBloodResistance;
	int breakEvenPoint;
	
	long totalFreeViruses;
	int totalWhiteBloodCount;
	int totalInfectedWhiteBloodCells;
	int totalUninfectedBodyCells;
	int totalInfectedBodyCells;
	int totalOrignalBodyCellCount;

	int difficulty = 1;
	int enemySpawnRate = 1;
	int playerSpawnRate = 1;
	
	int freeWhiteBloodCells = 0;
	long tickCount = 0;

    // which zone is currently selected, -1 is none
    int selected = -1;


    // Start is called before the first frame update
    void Start() {
        GameSettings newbieSettings = new GameSettings {
            // TODO
            freeVirusStart = 1000,
            whiteBloodStart = 20,
            bodyCells = 60000,
            infectBodyStart = 3000,
            deadVirusperWhiteBlood = 1500,
            deadWhiteBloodperDeadVirus = 0.0005,
            deadInfectedCellsperVirus = 0.4,
            infectedCellsperVirus = 0.055,
            virusesPerInfectedCell = 400,
            chanceICbursts = 0.1,
            spreadPerVirus = 0.005,
            whiteBloodResistance = 0.4,
            breakEvenPoint = 15000
        };
        GameSettings casualSettings = new GameSettings {
            freeVirusStart = 1000,
            whiteBloodStart = 20,
            bodyCells = 60000,
            infectBodyStart = 3000,
            deadVirusperWhiteBlood = 1500,
            deadWhiteBloodperDeadVirus = 0.0005,
            deadInfectedCellsperVirus = 0.4,
            infectedCellsperVirus = 0.055,
            virusesPerInfectedCell = 400,
            chanceICbursts = 0.1,
            spreadPerVirus = 0.005,
            whiteBloodResistance = 0.4,
            breakEvenPoint = 15000
        };
        GameSettings insaneSettings = new GameSettings {
            // TODO
            freeVirusStart = 1000,
            whiteBloodStart = 20,
            bodyCells = 60000,
            infectBodyStart = 3000,
            deadVirusperWhiteBlood = 1500,
            deadWhiteBloodperDeadVirus = 0.0005,
            deadInfectedCellsperVirus = 0.4,
            infectedCellsperVirus = 0.055,
            virusesPerInfectedCell = 400,
            chanceICbursts = 0.1,
            spreadPerVirus = 0.005,
            whiteBloodResistance = 0.4,
            breakEvenPoint = 15000
        };


        GameSettings[] settings = new GameSettings[3] {newbieSettings, casualSettings, insaneSettings};

        int diff = GlobalStaticVariables.getDiff();


        //Create nodes
        Node node1 = new Node(settings[diff]);
		Node node2 = new Node(settings[diff]);
        Node node3 = new Node(settings[diff]);
        Node node4 = new Node(settings[diff]);
        Node node5 = new Node(settings[diff]);
        Node node6 = new Node(settings[diff]);
        Node node7 = new Node(settings[diff]);
        Node node8 = new Node(settings[diff]);
        Node node9 = new Node(settings[diff]);
        Node node10 = new Node(settings[diff]);
        Node node11 = new Node(settings[diff]);

        //Adds nodes to nodeList
        nodeList.AddLast(node1);
		nodeList.AddLast(node2);
		nodeList.AddLast(node3);
		nodeList.AddLast(node4);
		nodeList.AddLast(node5);
		nodeList.AddLast(node6);
		nodeList.AddLast(node7);
		nodeList.AddLast(node8);
		nodeList.AddLast(node9);
		nodeList.AddLast(node10);
		nodeList.AddLast(node11);
		
		firstNode = nodeList.First;
		currentNode = firstNode;
		
		//Add adjacent nodes for node1
		currentNode.Value.adjacents.AddLast(node2);
		currentNode.Value.adjacents.AddLast(node3);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node2
		currentNode.Value.adjacents.AddLast(node1);
		currentNode.Value.adjacents.AddLast(node3);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node3
		currentNode.Value.adjacents.AddLast(node1);
		currentNode.Value.adjacents.AddLast(node2);
		currentNode.Value.adjacents.AddLast(node4);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node4
		currentNode.Value.adjacents.AddLast(node3);
		currentNode.Value.adjacents.AddLast(node5);
		currentNode.Value.adjacents.AddLast(node6);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node5
		currentNode.Value.adjacents.AddLast(node4);
		currentNode.Value.adjacents.AddLast(node6);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node6
		currentNode.Value.adjacents.AddLast(node4);
		currentNode.Value.adjacents.AddLast(node5);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node7
		currentNode.Value.adjacents.AddLast(node8);
		currentNode.Value.adjacents.AddLast(node9);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node8
		currentNode.Value.adjacents.AddLast(node7);
		currentNode.Value.adjacents.AddLast(node9);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node9
		currentNode.Value.adjacents.AddLast(node7);
		currentNode.Value.adjacents.AddLast(node8);
		currentNode.Value.adjacents.AddLast(node10);
		currentNode.Value.adjacents.AddLast(node11);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node10
		currentNode.Value.adjacents.AddLast(node9);
		currentNode.Value.adjacents.AddLast(node11);
		currentNode = currentNode.Next;
		
		//Add adjacent nodes for node11
		currentNode.Value.adjacents.AddLast(node9);
		currentNode.Value.adjacents.AddLast(node10);
		currentNode = currentNode.Next;
		
		
		currentNode = firstNode;
    }

    // Update is called once per frame
    void Update(){
		if(!paused){	//Checks if unpaused
			tickCount += 1;
			freeWhiteBloodCells += playerSpawnRate;
			
			//Temp variables for summing up node information
			long tempFreeViruses = 0;
			int tempWhiteBloodCount = 0;
			int tempInfectedWhiteBloodCells = 0;
			int tempUninfectedBodyCells = 0;
			int tempInfectedBodyCells = 0;
			int tempOrignalBodyCellCount = 0;
			
			//Iterates through linkedlist of nodes
			while(currentNode != null){
				currentNode.Value.tick();
				
				//Sums up information about each nodes
				tempFreeViruses += currentNode.Value.getFreeViruses();
				tempWhiteBloodCount += currentNode.Value.getWhiteBloodCount();
				tempInfectedWhiteBloodCells += currentNode.Value.getInfectedWhiteBloodCells();
				tempUninfectedBodyCells += currentNode.Value.getUninfectedBodyCells();
				tempInfectedBodyCells += currentNode.Value.getInfectedBodyCells();
				tempOrignalBodyCellCount += currentNode.Value.getOriginalCellCount();
				
				//Changes to next node
				currentNode = currentNode.Next;
				
			}
		
			totalFreeViruses = tempFreeViruses;
			totalWhiteBloodCount = tempWhiteBloodCount;
			totalInfectedWhiteBloodCells = tempInfectedWhiteBloodCells;
			totalUninfectedBodyCells = tempUninfectedBodyCells;
			totalInfectedBodyCells = tempInfectedBodyCells;
			totalOrignalBodyCellCount = tempOrignalBodyCellCount;
			
		}
    }
	
	public long getFreeViruses(){
		return (long)totalFreeViruses;
	}
	
	public int getWhiteBloodCount(){
		return (int)totalWhiteBloodCount;
	}
	
	public int getInfectedWhiteBloodCells(){
		return (int)totalInfectedWhiteBloodCells;
	}
	
	public int getUninfectedBodyCells(){
		return (int)totalUninfectedBodyCells;
	}
	
	public int getInfectedBodyCells(){
		return (int)totalInfectedBodyCells;
	}
	
	public int getOrignalBodyCellCount(){
		return (int)totalOrignalBodyCellCount;
	}

    public int getTotalViruses() {
        return (int)getFreeViruses() + getInfectedWhiteBloodCells() + getInfectedBodyCells();
    }
	
	public long getTickCount(){
		return tickCount;
	}

	public long getScore()
	{
		long a = getTickCount();
		return (a / getTotalViruses()) * 100000;
	}
	
    public double getHealth() {
        // returns a percentage of health
        return (double)getUninfectedBodyCells() / (double)getOrignalBodyCellCount();
    }

	public int getFreeWBC()
	{
		return freeWhiteBloodCells;
	}

	public void pauseGame(){
		paused = true;
	}
	
	public void unpauseGame(){
		paused = false;
	}
	
}
