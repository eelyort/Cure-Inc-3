using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
	//Variables
	LinkedList<Node> nodeList = new LinkedList<Node>();
	LinkedListNode<Node> firstNode;
	LinkedListNode<Node> currentNode;
	
	int freeVirusStart; 
	int whiteBloodStart; 
	int bodyCells;
	int infectBodyStart;
	double deadVirusperWhiteBlood;
	double deadWhiteBloodperDeadVirus;
	double deadInfectedCellsperVirus;
	double infectedCellsperVirus;
	double virusesPerInfectedCell;
	double chanceICbursts;
	double spreadPerVirus;
	double whiteBloodResistance;
	int breakEvenPoint;
	
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

	int difficulty = 1;
	int enemySpawnRate = 1;
	int playerSpawnRate = 1;
	
	int freeWhiteBloodCells = 0;
	
	
	
	// Start is called before the first frame update
    void Start()
    {
        //Sets up first node and currentNode variable for iteration
		firstNode = nodeList.First;
		currentNode = firstNode;
		nodeList.AddFirst(node1);
		nodeList.AddFirst(node2);
		nodeList.AddFirst(node3);
		nodeList.AddFirst(node4);
		nodeList.AddFirst(node5);
		nodeList.AddFirst(node6);
		nodeList.AddFirst(node7);
		nodeList.AddFirst(node8);
		nodeList.AddFirst(node9);
		nodeList.AddFirst(node10);
		nodeList.AddFirst(node11);
    }

    // Update is called once per frame
    void Update()
    {
		freeWhiteBloodCells += playerSpawnRate;
		while(currentNode != null){
			currentNode.Value.tick();
			currentNode = currentNode.Next;
		}
		
    }
}
