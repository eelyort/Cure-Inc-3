using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
	//Variables
	LinkedList<int> nodeList = new LinkedList<int>();
	LinkedListNode<int> firstNode;
	LinkedListNode<int> currentNode;
	
	int difficulty = 1;
	int enemySpawnRate = 1;
	int playerSpawnRate = 1;
	int tickCount = 0;
	
	
	
	
	
	// Start is called before the first frame update
    void Start()
    {
        //Sets up first node and currentNode variable for iteration
		firstNode = nodeList.First;
		currentNode = firstNode;
		
		//TODO: Set up variable values depending on difficulty
		
    }

    // Update is called once per frame
    void Update()
    {
		currentNode = currentNode.Next;
		
    }
}
