using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
	//Node variables
	LinkedList<Node> nodeList = new LinkedList<Node>();
	LinkedListNode<Node> firstNode;
	LinkedListNode<Node> currentNode;
	Text scoreText = GameObject.Find("Canvas/Text").GetComponent<Text>();
	
	bool paused = false;
	
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
    void Start(){
		//Create nodes
        Node node1 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node2 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node3 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node4 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node5 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node6 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node7 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node8 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node9 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node10 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		Node node11 = new Node(freeVirusStart, whiteBloodStart, bodyCells, infectBodyStart, deadVirusperWhiteBlood, deadWhiteBloodperDeadVirus, deadInfectedCellsperVirus, infectedCellsperVirus, virusesPerInfectedCell, chanceICbursts, spreadPerVirus, whiteBloodResistance, breakEvenPoint);
		
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
		
		//Updates score and free white blood cells text
		scoreText.text = "SCORE: " + getScore() + "\nFREE WHITE BLOOD CELLS REMAINING: " + freeWhiteBloodCells;
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
